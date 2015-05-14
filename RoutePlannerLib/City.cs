using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class City
    {
        private readonly double latitude;
        public string Name { get; private set; }
        public string Country { get; private set; }
        public int Population { get; private set; }
        public WayPoint Location { get; private set; }
        [XmlIgnore]
        public int Index { get; set; }

        public City() 
        {
            this.latitude = 0.0;
            Name = "";
            Country = "";
            Population = 0;
            Location = new WayPoint();
        }

        public City(string name, string country, int population, double latitude, double longitude)
        {
            this.latitude = latitude;
            Name = name;
            Country = country;
            Population = population;
            Location = new WayPoint(Name, latitude, longitude);
        }

        public override bool Equals(object obj)
        {
            City c = obj as City;
            if (c == null)
            {
                return false;
            }
            return Name == c.Name && Country == c.Country;
        }
    }
}
