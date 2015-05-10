using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util;
using System.Diagnostics;


namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class Cities
    {
        private readonly List<City> cityList;
        private static TraceSource CitiesLogger { get; set; }

        public Cities()
        {
            cityList = new List<City>();
            CitiesLogger = new TraceSource("Cities");
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
            return (
                from city in cityList
                where location.Distance(city.Location) <= distance
                orderby location.Distance(city.Location)
                select city
            ).ToList();
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
            CitiesLogger.TraceEvent(TraceEventType.Information, 1, "ReadCities started");

            using (TextReader reader = new StreamReader(filename))
            {
                var lines = reader.GetSplittedLines('\t');
                var count0 = cityList.Count;
                cityList.AddRange(
                    from l in lines
                    select new City(
                        name: l[0],
                        country: l[1],
                        population: Int32.Parse(l[2], CultureInfo.InvariantCulture),
                        laltitude: Double.Parse(l[3], CultureInfo.InvariantCulture),
                        longitude: Double.Parse(l[4], CultureInfo.InvariantCulture))
                );
                CitiesLogger.TraceEvent(TraceEventType.Information, 2, "ReadCities ended");
                return cityList.Count - count0;
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
            return InitIndexForAlgorithm(foundCities);
        }

        /// <summary>
        /// Create index for eatch city
        /// </summary>
        /// <param name="foundCities">List of found cities</param>
        /// <returns>same object but indexed</returns>
        private List<City> InitIndexForAlgorithm(List<City> foundCities)
        {
            // set index for FloydWarshall 
            for (var index = 0; index < foundCities.Count; index++)
                foundCities[index].Index = index;

            return foundCities;
        } 
    }
}
