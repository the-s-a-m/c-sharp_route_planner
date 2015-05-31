
using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    /// <summary>
    /// Manages a links from a city to another city.
    /// </summary>
    public abstract class Routes : IRoutes
    {
        private readonly List<Link> links = new List<Link>();
        protected Cities Cities { get; private set; }

        public delegate void RouteRequestHandler(object sender, RouteRequestEventArgs e);
        public event RouteRequestHandler RouteRequestEvent;
        private static readonly TraceSource RoutesLogger = new TraceSource("Routes");
        public bool ExecuteParallel { set; get; } 

        /// <summary>
        /// Initializes the Routes with the cities.
        /// </summary>
        /// <param name="cities"></param>
        protected Routes(Cities cities)
        {
            this.Cities = cities;
        }

        public void NotifyObservers(Object form, RouteRequestEventArgs args)
        {
            if (RouteRequestEvent != null)
            {
                RouteRequestEvent(form, args);
            }
        }

        public int Count
        {
            get { return Links.Count; }
        }

        protected List<Link> Links
        {
            get { return links; }
        }

        /// <summary>
        /// Reads a list of links from the given file.
        /// Reads only links where the cities exist.
        /// </summary>
        /// <param name="filename">name of links file</param>
        /// <returns>number of read route</returns>
        public int ReadRoutes(string filename)
        {
            RoutesLogger.TraceEvent(TraceEventType.Information, 3, "ReadRoutes started");
            try
            {
                TextReader reader = new StreamReader(filename);

                foreach (var line in reader.GetSplittedLines('\t'))
                {
                    City city1 = Cities.FindCity(line[0].Trim(' '));
                    City city2 = Cities.FindCity(line[1].Trim(' '));

                    // only add links, where the cities are found 
                    if ((city1 != null) && (city2 != null))
                    {
                        Links.Add(new Link(city1, city2, city1.Location.Distance(city2.Location), TransportModes.Rail));
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                RoutesLogger.TraceEvent(TraceEventType.Critical, 9, e.ToString());
            }

            RoutesLogger.TraceEvent(TraceEventType.Information, 4, "ReadRoutes ended");
            return Count;

        }

        public abstract List<Link> FindShortestRouteBetween(string fromCity, string toCity, TransportModes mode, IProgress<string> reportProgress);

        protected List<Link> FindPath(List<City> citiesOnRoute, TransportModes mode)
        {
            var citiesAsLinks = new List<Link>(citiesOnRoute.Count);
            for(var i = 0; i < citiesOnRoute.Count - 1; i++)
            {
                City c1 = citiesOnRoute[i];
                City c2 = citiesOnRoute[i + 1];
                citiesAsLinks.Add(new Link(c1, c2, c1.Location.Distance(c2.Location)));
            }
           
            return citiesAsLinks;
        }

        protected static List<City> FillListOfNodes(List<City> cities, out Dictionary<City, double> dist, out Dictionary<City, City> previous)
        {
            var q = new List<City>(); // the set of all nodes (cities) in Graph ;
            dist = new Dictionary<City, double>();
            previous = new Dictionary<City, City>();
            foreach (var v in cities)
            {
                dist[v] = double.MaxValue;
                previous[v] = null;
                q.Add(v);
            }

            return q;
        }

        //F�r was wird der transportMode �bergeben, dieser ist als readonly markiert im waypoint?
        protected Link FindLink(City u, City n, TransportModes mode)
        {
            return u != null && n != null ? new Link(u, n, u.Location.Distance(n.Location)) : null;
        }

        public Task<List<Link>> FindShortestRouteBetweenAsync(string fromCity, string toCity, TransportModes mode)
        {
            return Task.Run(() => FindShortestRouteBetween(fromCity, toCity, mode, null));
        }

        public List<Link> GoFindShortestRouteBetween(string fromCity, string toCity, TransportModes mode)
        {
            var task = FindShortestRouteBetweenAsync(fromCity, toCity, mode);
            task.Wait();
            return task.Result;
        }

        /// <summary>
        /// Weiterleitung auf FindshortestRouteBetween
        /// </summary>
        /// <param name="fromCity"></param>
        /// <param name="toCity"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public List<Link> FindShortestRouteBetween(string fromCity, string toCity, TransportModes mode)
        {
            return FindShortestRouteBetween(fromCity, toCity, mode, null);
        } 
        

        protected List<City> GetCitiesOnRoute(City source, City target, Dictionary<City, City> previous)
        {
            var citiesOnRoute = new List<City>();
            var cr = target;
            while (previous[cr] != null)
            {
                citiesOnRoute.Add(cr);
                cr = previous[cr];
            }
            citiesOnRoute.Add(source);

            citiesOnRoute.Reverse();
            return citiesOnRoute;
        }

        /// <summary>
        /// Returns a set (unique entries) of cities, which have links with this transport mode.
        /// </summary>
        /// <param name="transportMode"></param>
        /// <returns></returns>
        public City[] FindCities(TransportModes transportMode)
        {
            return Links.Where(r => r.TransportMode == transportMode).SelectMany(r => new City[]{ r.FromCity, r.ToCity }).Distinct().ToArray();
        }
    }
}
