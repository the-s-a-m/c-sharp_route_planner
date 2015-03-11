using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util;


namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class Cities
    {
        private readonly List<City> cityList;

        public Cities()
        {
            cityList = new List<City>();
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
                return cityList[index];
            }
            set { cityList[index] = value; }
        }

        /// <summary>
        /// Returns all cities, which have a distance &lt;= the given distance from the given location.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="distance"></param>
        /// <returns>List of cities which match the condtitions.</returns>
        public List<City> FindNeighbours(WayPoint location, double distance)
        {
            var foundCities = cityList.Where(c => location.Distance(c.Location) <= distance);
            return foundCities.OrderBy(o => location.Distance(o.Location)).ToList();
        }

        /// <summary>
        /// Number of elements contained by this instance.
        /// </summary>
        public int Count
        {
            get
            {
                return cityList.Count;
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

                /* Ging so nicht
                citiesAsStrings.ToList().ForEach(s => cityList.Add(new City(s[0], s[1], int.Parse(s[2], CultureInfo.InvariantCulture), 
                    double.Parse(s[3], CultureInfo.InvariantCulture), double.Parse(s[4], CultureInfo.InvariantCulture))));
                */
                int count = 0;
                foreach(var c in reader.GetSplittedLines('\t'))
                {
                    cityList.Add(new City(c[0], c[1], int.Parse(c[2], CultureInfo.InvariantCulture),
                        double.Parse(c[3], CultureInfo.InvariantCulture), double.Parse(c[4], CultureInfo.InvariantCulture)));
                    ++count;
                }
                return count;


                /* Old Code
                int count = 0;
                String c = reader.ReadLine();
                while (c != null)
                {
                    String[] cSplit = c.Split('\t');
                    City newCity = new City(cSplit[0], cSplit[1], Convert.ToInt32(cSplit[2]),
                        Convert.ToDouble(cSplit[3]), Convert.ToDouble(cSplit[4]));
                    cityList.Add(newCity);
                    ++count;
                    c = reader.ReadLine();
                }
                return count;
                 */
            }
        }

        /// <summary>
        /// Searches for a city with the given name. (Case insensitive)
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns></returns>
        public City FindCity(string cityName)
        {
            return cityList.Find(c => c.Name.Equals(cityName , StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Find all cities between 2 cities 
        /// </summary>
        /// <param name="from">source city</param>
        /// <param name="to">target city</param>
        /// <returns>list of cities</returns>
        public List<City> FindCitiesBetween(City from, City to)
        {
            var foundCities = new List<City>();
            if (from == null || to == null)
            {
                return foundCities;
            }

            foundCities.Add(from);

            var minLat = Math.Min(from.Location.Latitude, to.Location.Latitude);
            var maxLat = Math.Max(from.Location.Latitude, to.Location.Latitude);
            var minLon = Math.Min(from.Location.Longitude, to.Location.Longitude);
            var maxLon = Math.Max(from.Location.Longitude, to.Location.Longitude);

            // rename the name of the "cities" variable to your name of the internal City-List
            foundCities.AddRange(cityList.FindAll(c =>
                c.Location.Latitude > minLat && c.Location.Latitude < maxLat
                        && c.Location.Longitude > minLon && c.Location.Longitude < maxLon));

            foundCities.Add(to);
            return foundCities;
        }
    }
}
