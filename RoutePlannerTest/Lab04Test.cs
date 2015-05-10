using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib;


namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerTest
{
    [TestClass]
    [DeploymentItem("data/citiesTestDataLab4.txt")]
    [DeploymentItem("data/linksTestDataLab4.txt")]
    public class Lab4Test
    {
        private const string CitiesTestFile = "citiesTestDataLab4.txt";
        private const string LinksTestFile = "linksTestDataLab4.txt";

        [TestMethod]
        public void TestWayPointCalculation()
        {
            var wp1 = new WayPoint("Home", 10.4, 20.8);
            var wp2 = new WayPoint("Target", 1.2, 2.4);

            var addWp = wp1 + wp2;
            Assert.AreEqual(wp1.Name, addWp.Name);
            Assert.AreEqual(wp1.Latitude + wp2.Latitude, addWp.Latitude);
            Assert.AreEqual(wp1.Longitude + wp2.Longitude, addWp.Longitude);

            var minWp = wp1 - wp2;
            Assert.AreEqual(wp1.Name, minWp.Name);
            Assert.AreEqual(wp1.Latitude - wp2.Latitude, minWp.Latitude);
            Assert.AreEqual(wp1.Longitude - wp2.Longitude, minWp.Longitude);
        
        }

        [TestMethod]
        public void TestTaskReadRoutes()
        {
            var cities = new Cities();
            cities.ReadCities(CitiesTestFile);
            var expectedLinks = new List<Link>();
            expectedLinks.Add(new Link(new City("Zürich", "Switzerland", 7000, 1, 2),
                                       new City("Aarau", "Switzerland", 7000, 1, 2), 0));
            expectedLinks.Add(new Link(new City("Aarau", "Switzerland", 7000, 1, 2),
                                       new City("Liestal", "Switzerland", 7000, 1, 2), 0));
            expectedLinks.Add(new Link(new City("Liestal", "Switzerland", 7000, 1, 2),
                                       new City("Basel", "Switzerland", 7000, 1, 2), 0));

            var routes = new RoutesDijkstra(cities);
            var count = routes.ReadRoutes(LinksTestFile);

            Assert.AreEqual(10, count);
            Assert.AreEqual(10, routes.Count);

        }


        [TestMethod]
        public void TestTask4FindRoutes()
        {
            var cities = new Cities();
            cities.ReadCities(CitiesTestFile);
            var expectedLinks = new List<Link>();
            expectedLinks.Add(new Link(new City("Zürich", "Switzerland", 7000, 1,2), 
                                       new City("Aarau", "Switzerland", 7000, 1,2), 0));
            expectedLinks.Add(new Link(new City("Aarau", "Switzerland", 7000, 1, 2),
                                       new City("Liestal", "Switzerland", 7000, 1, 2), 0));
            expectedLinks.Add(new Link(new City("Liestal", "Switzerland", 7000, 1, 2),
                                       new City("Basel", "Switzerland", 7000, 1, 2), 0));

            var routes = new RoutesDijkstra(cities);
            routes.ReadRoutes(LinksTestFile);

            Assert.AreEqual(11, cities.Count);

            // test available cities
            var links = routes.FindShortestRouteBetween("Zürich", "Basel", TransportModes.Rail); 
            
            Assert.IsNotNull(links);
            Assert.AreEqual(expectedLinks.Count, links.Count);

            for (var i = 0; i < links.Count; i++)
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



    }
    
}
