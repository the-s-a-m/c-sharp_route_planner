using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib;
using System.IO;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util;
using System.Diagnostics;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerConsole
{
    class RoutePlannerConsoleApp
    {
        private const string CitiesTestFile = "citiesTestDataLab3.txt";
        private static TraceSource routesLogger = new TraceSource("Routes");
        private static readonly Stopwatch sw = new Stopwatch();

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
            cities.ReadCities("citiesTestDataLab4.txt");
            IRoutes routes = RoutesFactory.Create(cities);
            Console.WriteLine(routes);
            var count = routes.ReadRoutes("linksTestDataLab4.txt");

            //TestError Loading Lab9 b)
            var count2 = routes.ReadRoutes("linksTestDataLab42.txt");

            var c1 = new City("Aarau", "Switzerland", 10, 1.1, 2.2);
            var c2 = new City("Bern", "Switzerland", 10, 1.1, 2.2);

            Console.WriteLine("\nsimpleObjectWriterTest");
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

            //Lab9 a1 c) Console & File Test of Readcities
            var cities3 = new Cities();
            cities3.ReadCities("citiesTestDataLab4.txt");
            IRoutes routes2 = RoutesFactory.Create(cities);

            //Lab9 a1 b) Loading from existing file
            var count3 = routes.ReadRoutes("linksTestDataLab4.txt");

            //Lab9 a1 b) Writing to file but not to console
            routesLogger.TraceEvent(TraceEventType.Information, 01, "this should not be on the console");


            //Lab9 a1 b) Loding not existing file
            var count4 = routes.ReadRoutes("linksTestDataLab42.txt");
            
            //Lab10 Tests
            Console.WriteLine("Lab10 Tests");
            Cities c10 = new Cities();
            c10.ReadCities(@"citiesTestDataLab10.txt");
            Console.WriteLine(c10.Count);

            Routes r10 = new RoutesFloydWarshall(cities);
            int count10 = r10.ReadRoutes(@"linksTestDataLab10.txt");
            Console.WriteLine(count10);

            // test short routes in parallel mode
            r10.ExecuteParallel = true;
            sw.Start();
            List<Link> links = routes.FindShortestRouteBetween("Lyon", "Berlin", TransportModes.Rail);
            sw.Stop();
            Console.WriteLine("Parallel: " + sw.ElapsedMilliseconds);

            // test short routes in seqential mode
            r10.ExecuteParallel = false;
            sw.Restart();
            List<Link> links2 = routes.FindShortestRouteBetween("Lyon", "Berlin", TransportModes.Rail);
            sw.Stop();
            Console.WriteLine("Sequential: " + sw.ElapsedMilliseconds);

            //feststellung Parallel benötigt länger

            Console.ReadLine();
        }
    }
}
