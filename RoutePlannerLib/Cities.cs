using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    class Cities
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
            while (c != null)
            {
                String[] cSplit = c.Split('\t');
                citiesList.Add(new City(cSplit[0], cSplit[1], Convert.ToInt32(cSplit[2]), Convert.ToDouble(cSplit[3]), Convert.ToDouble(cSplit[4])));
                c = reader.ReadLine();
            }
            return citiesList.Count;
        }
    }
}
