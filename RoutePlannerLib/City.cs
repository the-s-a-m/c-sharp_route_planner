using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class City
    {
        public string Name { get; private set; }
        public string Country { get; private set; }
        public int Population { get; private set; }
        public WayPoint Location { get; private set; }

        public City(string _name, string _country, int _population, double _laltitude, double _longitude)
        {
            Name = _name;
            Country = _country;
            Population = _population;
            Location = new WayPoint(Name, _laltitude, _longitude);
        }

        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            City c = obj as City;
            if ((System.Object)c == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (Name == c.Name) && (Country == c.Country) && (Population == c.Population) && (Location.Equals(c.Location));
        }
    }
}
