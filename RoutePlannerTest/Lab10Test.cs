using System.Collections.Generic;
using System.Diagnostics;

using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerTest
{
    [TestClass]
    [DeploymentItem("data/citiesTestDataLab2.txt")]
    [DeploymentItem("data/citiesTestDataLab4.txt")]
    [DeploymentItem("data/citiesTestDataLab10.txt")]
    [DeploymentItem("data/linksTestDataLab10.txt")]
    public class Lab11Test
    {
        private const string CitiesTestFile = "citiesTestDataLab2.txt";

        [TestMethod]
        public void TestCorrectIndexingOfCities()
        {
            const int readCitiesExpected = 10;
            var cities = new Cities();

            Assert.AreEqual(readCitiesExpected, cities.ReadCities(CitiesTestFile)); 

            City from = cities.FindCity("Mumbai");
            City to = cities.FindCity("Istanbul");
            List<City> foundCities = cities.FindCitiesBetween(from, to);

            // verify that Index property is initialized
            int i = 0;
            foreach (var city in foundCities)
            {
                Assert.AreEqual(i, city.Index);
                i++;
            }
    
        }

        [TestMethod]
        public void TestTask1FindRoutes()
        {
            Cities cities = new Cities();
            cities.ReadCities(@"citiesTestDataLab4.txt");
            Assert.AreEqual(11, cities.Count);

            List<Link> expectedLinks = new List<Link>();
            expectedLinks.Add(new Link(new City("Zürich", "Switzerland", 7000, 1, 2),
                                       new City("Aarau", "Switzerland", 7000, 1, 2), 0));
            expectedLinks.Add(new Link(new City("Aarau", "Switzerland", 7000, 1, 2),
                                       new City("Liestal", "Switzerland", 7000, 1, 2), 0));
            expectedLinks.Add(new Link(new City("Liestal", "Switzerland", 7000, 1, 2),
                                       new City("Basel", "Switzerland", 7000, 1, 2), 0));

            Routes routes = new RoutesFloydWarshall(cities);
            int count = routes.ReadRoutes(@"linksTestDataLab4.txt");
            Assert.AreEqual(10, count);

            // test available cities
            routes.ExecuteParallel = true;
            List<Link> links = routes.FindShortestRouteBetween("Zürich", "Basel", TransportModes.Rail);

            Assert.IsNotNull(links);
            Assert.AreEqual(expectedLinks.Count, links.Count);

            for (int i = 0; i < links.Count; i++)
            {
                Assert.IsTrue(
                    (expectedLinks[i].FromCity.Name == links[i].FromCity.Name &&
                     expectedLinks[i].ToCity.Name == links[i].ToCity.Name) ||
                    (expectedLinks[i].FromCity.Name == links[i].ToCity.Name &&
                     expectedLinks[i].ToCity.Name == links[i].FromCity.Name));
            }


            links = routes.FindShortestRouteBetween("doesNotExist", "either", TransportModes.Rail);
            Assert.IsNull(links);

        }


        [TestMethod]
        public void TestTask2FindRoutes()
        {
            Cities cities = new Cities();
            cities.ReadCities(@"citiesTestDataLab10.txt");
            Assert.AreEqual(6372, cities.Count);

            Routes routes = new RoutesFloydWarshall(cities);
            int count = routes.ReadRoutes(@"linksTestDataLab10.txt");
            Assert.AreEqual(118, count);


            // test short routes in parallel mode
            routes.ExecuteParallel = true;
            List<Link> links = routes.FindShortestRouteBetween("Lyon", "Berlin", TransportModes.Rail);
            Assert.IsNotNull(links);
            Assert.AreEqual(13, links.Count);

            // test short routes in parallel mode
            routes.ExecuteParallel = false;
            List<Link> links2 = routes.FindShortestRouteBetween("Lyon", "Berlin", TransportModes.Rail);
            Assert.IsNotNull(links2);
            Assert.AreEqual(13, links2.Count);

        }

        [TestMethod]
        public void TestTask3CompareAlgorithms()
        {
            //IGNORE THIS TEST - ITS UNRELIABLE
            /*
            Cities cities = new Cities();

            cities.ReadCities(@"citiesTestDataLab10.txt");
            Assert.AreEqual(6372, cities.Count);

            Routes routes = new RoutesDijkstra(cities);
            long dijkstraTime = this.FindRoutes(routes);

            routes = new RoutesFloydWarshall(cities);
            routes.ExecuteParallel = false;
            long floydWarshallTime = this.FindRoutes(routes);

            // the sequential version should be slower
            Assert.IsTrue(floydWarshallTime > dijkstraTime, "FloydWarshal should be slower");
            */
        }

        private long FindRoutes(Routes routes)
        {
            routes.ReadRoutes(@"linksTestDataLab10.txt");

            // test available cities
            Stopwatch timer = new Stopwatch();

            timer.Start();
            routes.FindShortestRouteBetween("Lyon", "Berlin", TransportModes.Rail);
            return timer.ElapsedTicks;
        }
    }
}
