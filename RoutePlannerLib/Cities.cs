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
        private List<City> citiesList;

        public Cities()
        {
            citiesList = new List<City>();
        }

        public City this[int index]
        {
            get 
            {
                if (index < 0 || index > this.Count)
                {
                    return null;
                }
                else
                {
                    return citiesList[index]; 
                }
            }
            set { citiesList[index] = value; }
        }

        public List<City> FindNeighbours(WayPoint location, double distance)
        {
            var foundCities = new List<City>();
            foreach(var c in citiesList) 
            {
                if (location.Distance(c.Location) <= distance)
                {
                    foundCities.Add(c);
                }
            }
            return foundCities.OrderBy(o => location.Distance(o.Location)).ToList();
        }

        public int Count
        {
            get
            {
                return citiesList.Count;
            }
        }

        public int ReadCities(string filename)
        {
            TextReader reader = new StreamReader(filename);
            String c = reader.ReadLine();
            int count = 0;
            while (c != null)
            {
                String[] cSplit = c.Split('\t');
                City newCity = new City(cSplit[0], cSplit[1], Convert.ToInt32(cSplit[2]), Convert.ToDouble(cSplit[3]), Convert.ToDouble(cSplit[4]));
                //if (!citiesList.Contains(newCity))
                //{
                    citiesList.Add(newCity);
                    count++;
                //}
                c = reader.ReadLine();
            }
            return count;
        }

        public City FindCity(string cityName)
        {
            return citiesList.Find(c => c.Name.Equals(cityName , StringComparison.OrdinalIgnoreCase));
        }
    }
}
