
using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util;
using System.Diagnostics;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    /// <summary>
    /// Manages a routes from a city to another city.
    /// </summary>
    public abstract class Routes : IRoutes
    {
        protected readonly List<Link> routes = new List<Link>();
        protected readonly Cities cities;
        public delegate void RouteRequestHandler(object sender, RouteRequestEventArgs e);
        public event RouteRequestHandler RouteRequestEvent;
        private static TraceSource routesLogger = new TraceSource("Routes");

        /// <summary>
        /// Initializes the Routes with the cities.
        /// </summary>
        /// <param name="cities"></param>
        public Routes(Cities cities)
        {
            this.cities = cities;
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
            get { return routes.Count; }
        }

        /// <summary>
        /// Reads a list of links from the given file.
        /// Reads only links where the cities exist.
        /// </summary>
        /// <param name="filename">name of links file</param>
        /// <returns>number of read route</returns>
        public int ReadRoutes(string filename)
        {
            routesLogger.TraceEvent(TraceEventType.Information, 3, "ReadRoutes started");
            try
            {
                TextReader reader = new StreamReader(filename);

                foreach (var line in reader.GetSplittedLines('\t'))
                {

                    City city1 = cities.FindCity(line[0]);
                    City city2 = cities.FindCity(line[1]);

                    // only add links, where the cities are found 
                    if ((city1 != null) && (city2 != null))
                    {
                        routes.Add(new Link(city1, city2, city1.Location.Distance(city2.Location), TransportModes.Rail));
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                routesLogger.TraceEvent(TraceEventType.Critical, 9, e.ToString());
            }

            routesLogger.TraceEvent(TraceEventType.Information, 4, "ReadRoutes ended");
            return Count;

        }

        public abstract List<Link> FindShortestRouteBetween(string fromCity, string toCity, TransportModes mode);

        protected List<Link> FindPath(List<City> citiesOnRoute, TransportModes mode)
        {
            var citiesAsLinks = new List<Link>(citiesOnRoute.Count);
            for(int i = 0; i < citiesOnRoute.Count - 1; i++)
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


        //Für was wird der transportMode übergeben, dieser ist als readonly markiert im waypoint?
        protected Link FindLink(City u, City n, TransportModes mode)
        {
            return u != null && n != null ? new Link(u, n, u.Location.Distance(n.Location)) : null;
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
        /// Returns a set (unique entries) of cities, which have routes with this transport mode.
        /// </summary>
        /// <param name="transportMode"></param>
        /// <returns></returns>
        public City[] FindCities(TransportModes transportMode)
        {
            return routes.Where(r => r.TransportMode == transportMode).SelectMany(r => new City[]{ r.FromCity, r.ToCity }).Distinct().ToArray();
        }
    }
}
