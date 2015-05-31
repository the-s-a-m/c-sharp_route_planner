using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class RoutesDijkstra : Routes
    {
        public RoutesDijkstra(Cities cities): base(cities)
        {
        }
        public override List<Link> FindShortestRouteBetween(string fromCity, string toCity, TransportModes mode, 
            IProgress<string> reportProgress)
        {
            NotifyObservers(this, new RouteRequestEventArgs(fromCity, toCity, mode));
            ProgressReport(reportProgress, "<notified observers> done");

            var citiesBetween = Cities.FindCitiesBetween(Cities.FindCity(fromCity), Cities.FindCity(toCity));
            ProgressReport(reportProgress, "<found cities between> done");

            if (citiesBetween == null || citiesBetween.Count < 1 || Links == null || Links.Count < 1)
                return null;

            var source = citiesBetween[0];
            var target = citiesBetween[citiesBetween.Count - 1];

            Dictionary<City, double> dist;
            Dictionary<City, City> previous;
            var q = FillListOfNodes(citiesBetween, out dist, out previous);
            ProgressReport(reportProgress, "<created nodes> done");
            dist[source] = 0.0;

            // the actual algorithm
            previous = SearchShortestPath(mode, q, dist, previous);
            ProgressReport(reportProgress, "<search shortest path> done");

            // create a list with all cities on the route
            var citiesOnRoute = GetCitiesOnRoute(source, target, previous);
            ProgressReport(reportProgress, "<got cities on route> done");

            // prepare final list if links
            return FindPath(citiesOnRoute, mode);
        }

        private static void ProgressReport(IProgress<string> reportProgress, string report)
        {
            if (reportProgress != null)
            {
                reportProgress.Report(report);
            }
        }


        public Task<List<Link>> FindShortestRouteBetweenAsync(string fromCity, string toCity, TransportModes mode, Progress<string> progress)
        {
            return Task.Run(() => FindShortestRouteBetween(fromCity, toCity, mode, progress));
        }

        /// <summary>
        /// Searches the shortest path for cities and the given links
        /// </summary>
        /// <param name="mode">transportation mode</param>
        /// <param name="q"></param>
        /// <param name="dist"></param>
        /// <param name="previous"></param>
        /// <returns></returns>
        private Dictionary<City, City> SearchShortestPath(TransportModes mode, List<City> q, Dictionary<City, double> dist, Dictionary<City, City> previous)
        {
            while (q.Count > 0)
            {
                City u = null;
                var minDist = double.MaxValue;

                // find city u with smallest dist
                // also possible with q.Where(c => dist[c] < minDist)
                foreach (var c in q)
                {
                    if (dist[c] < minDist)
                    {
                        u = c;
                        minDist = dist[c];
                    }
                }

                if (u != null)
                {
                    q.Remove(u);
                    foreach (var n in FindNeighbours(u, mode))
                    {
                        var l = FindLink(u, n, mode);
                        var d = dist[u];
                        if (l != null)
                        {
                            d += l.Distance;
                        }
                        else
                        {
                            d += double.MaxValue;
                        }

                        if (dist.ContainsKey(n) && d < dist[n])
                        {
                            dist[n] = d;
                            previous[n] = u;
                        }
                    }
                }
                else
                {
                    break;
                }
            }
            return previous;
        }

        /// <summary>
        /// Finds all neighbor cities of a city. 
        /// </summary>
        /// <param name="city">source city</param>
        /// <param name="mode">transportation mode</param>
        /// <returns>list of neighbor cities</returns>
        private List<City> FindNeighbours(City city, TransportModes mode)
        {
            return (
                from route in Links
                where route.TransportMode.Equals(mode) && (route.FromCity.Equals(city) || route.ToCity.Equals(city))
                select route.FromCity.Equals(city) ? route.ToCity : route.FromCity
            ).ToList();
        }

    }
}
