using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class RouteRequestWatcher
    {
        private readonly Dictionary<string, int> routeCounter = new Dictionary<string, int>();

        public void LogRouteRequests(object sender, RouteRequestEventArgs e)
        {
            if (e.ToCity != null)
            {
                string toCityLowered = e.ToCity.ToLower();
                if (routeCounter.ContainsKey(toCityLowered))
                {
                    routeCounter[toCityLowered]++;
                }
                else
                {
                    routeCounter.Add(toCityLowered, 1);
                }
            }
        }

        public int GetCityRequests(string cityName)
        {
            if (cityName != null)
            {
                string cityLowered = cityName.ToLower();
                if (routeCounter.ContainsKey(cityLowered))
                {
                    return routeCounter[cityLowered];
                }
            }
            return 0;
        }
    }
}
