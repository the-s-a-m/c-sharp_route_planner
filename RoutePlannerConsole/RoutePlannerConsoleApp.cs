using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib;
using System.IO;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerConsole
{
    class RoutePlannerConsoleApp
    {
        private const string CitiesTestFile = "citiesTestDataLab3.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to RoutePlanner (Version " +
                System.Reflection.Assembly.GetExecutingAssembly().GetName().Version + 
                ") ");
            var wayPoint = new WayPoint("Windisch", 47.479319847061966, 8.212966918945312);
            Console.WriteLine("{0}: {1}/{2}", wayPoint.Name, wayPoint.Latitude, wayPoint.Longitude);
            Console.WriteLine(wayPoint.ToString());
            var bern = new WayPoint("Bern", 46.949690, 7.442420);
            var tripolis = new WayPoint("Tripolis", 32.815062, 13.105445);
            Console.WriteLine(bern.Distance(tripolis));

            var cities = new Cities();
            IRoutes routes = RoutesFactory.Create(cities);
            Console.WriteLine(routes);

            var c1 = new City("Aarau", "Switzerland", 10, 1.1, 2.2);
            var c2 = new City("Bern", "Switzerland", 10, 1.1, 2.2);

            var stream = new StringWriter();
            var writer = new SimpleObjectWriter(stream);
            writer.Next(c1);
            Console.WriteLine(stream.ToString());

            Console.WriteLine("readTest Lab5");
            const string cityString1 = "Instance of Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.City\r\nName=\"Aarau\"\r\nCountry=\"Switzerland\"\r\nPopulation=10\r\nLocation is a nested object...\r\nInstance of Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.WayPoint\r\nName=\"Aarau\"\r\nLongitude=2.2\r\nLatitude=1.1\r\nEnd of instance\r\nEnd of instance\r\n";
            const string cityString2 = "Instance of Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.City\r\nName=\"Bern\"\r\nCountry=\"Switzerland\"\r\nPopulation=10\r\nLocation is a nested object...\r\nInstance of Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.WayPoint\r\nName=\"Bern\"\r\nLongitude=2.2\r\nLatitude=1.1\r\nEnd of instance\r\nEnd of instance\r\n";
            const string cityString = cityString1 + cityString2;
            var expectedCity1 = new City("Aarau", "Switzerland", 10, 1.1, 2.2);
            var expectedCity2 = new City("Bern", "Switzerland", 10, 1.1, 2.2);
            var stream2 = new StringReader(cityString);
            var reader = new SimpleObjectReader(stream2);
            var city1 = reader.Next() as City;
            if (city1 == null)
            {
                Console.WriteLine("city was null");
            }
            Console.WriteLine(city1.ToString());

            Console.WriteLine("ActionTest Lab6");
            var actions = new Action[3];
            for (var i = 0; i < actions.Length; i++)
            {
                var z = i;
                actions[i] = () => Console.Write(z);
            }

            foreach (var a in actions)
            {
                a();
            } 

            Console.ReadLine();
        }
    }
}
