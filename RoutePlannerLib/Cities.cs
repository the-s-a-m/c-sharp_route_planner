using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class Cities
    {
        private readonly List<City> citiesList;

        public Cities()
        {
            citiesList = new List<City>();
        }

        /// <summary>
        /// Index based access to the individual city objects.
        /// </summary>
        /// <param name="index">Numerical index in the range [0, Cities.Count)</param>
        /// <returns></returns>
        public City this[int index]
        {
            get
            {
                if (index < 0 || index > this.Count)
                {
                    return null;
                }
                return citiesList[index];
            }
            set { citiesList[index] = value; }
        }

        /// <summary>
        /// Returns all cities, which have a distance &lt;= the given distance from the given location.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="distance"></param>
        /// <returns>List of cities which match the condtitions.</returns>
        public List<City> FindNeighbours(WayPoint location, double distance)
        {
            var foundCities = citiesList.Where(c => location.Distance(c.Location) <= distance).ToList();
            return foundCities.OrderBy(o => location.Distance(o.Location)).ToList();
        }

        /// <summary>
        /// Number of elements contained by this instance.
        /// </summary>
        public int Count
        {
            get
            {
                return citiesList.Count;
            }
        }

        /// <summary>
        /// Reads cities from the given file into this instance
        /// </summary>
        /// <param name="filename">Filename of the city source</param>
        /// <returns>Number of new cities</returns>
        public int ReadCities(string filename)
        {
            using (TextReader reader = new StreamReader(filename))
            {
                int count = 0;
                String c = reader.ReadLine();
                while (c != null)
                {
                    String[] cSplit = c.Split('\t');
                    City newCity = new City(cSplit[0], cSplit[1], Convert.ToInt32(cSplit[2]),
                        Convert.ToDouble(cSplit[3]), Convert.ToDouble(cSplit[4]));
                    citiesList.Add(newCity);
                    ++count;
                    c = reader.ReadLine();
                }
                return count;
            }
        }

        /// <summary>
        /// Searches for a city with the given name. (Case insensitive)
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns></returns>
        public City FindCity(string cityName)
        {
            return citiesList.Find(c => c.Name.Equals(cityName , StringComparison.OrdinalIgnoreCase));
        }
    }
}
