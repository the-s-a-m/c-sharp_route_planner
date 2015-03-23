using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class City
    {
        private readonly double laltitude;
        public string Name { get; private set; }
        public string Country { get; private set; }
        public int Population { get; private set; }
        public WayPoint Location { get; private set; }

        public City() 
        {
            this.laltitude = 0.0;
            Name = "";
            Country = "";
            Population = 0;
            Location = new WayPoint();
        }

        public City(string name, string country, int population, double laltitude, double longitude)
        {
            this.laltitude = laltitude;
            Name = name;
            Country = country;
            Population = population;
            Location = new WayPoint(Name, laltitude, longitude);
        }

    }
}
