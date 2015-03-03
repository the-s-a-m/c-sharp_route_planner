using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib;
using System;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerTest
{
    [TestClass]
    [DeploymentItem("data/citiesTestDataLab3.txt")]
    [DeploymentItem("data/linksTestDataLab3.txt")]
    public class Lab3Test
    {
        private const string CitiesTestFile = "citiesTestDataLab3.txt";
        private const string LinksTestFile = "linksTestDataLab3.txt";

        [TestMethod]
        public void TestLinkTransportMode()
        {
            var mumbai = new City("Mumbai", "India", 12383146, 18.96, 72.82);
            var buenosAires = new City("Buenos Aires", "Argentina", 12116379, -34.61, -58.37);

            var link = new Link(mumbai, buenosAires, 10);
            // verify default transport
            Assert.AreEqual(TransportModes.Car, link.TransportMode);

            link = new Link(mumbai, buenosAires, 10, TransportModes.Ship);
            Assert.AreEqual(TransportModes.Ship, link.TransportMode);
        }

        [TestMethod]
        public void TestTask1FindCityInCities()
        {
            var expectedCity = "Zürich";
            Cities cities = new Cities();
            cities.ReadCities(CitiesTestFile);

            var notFound = cities.FindCity("noCity");
            Assert.IsNull(notFound);
            var found = cities.FindCity(expectedCity);
            Assert.AreEqual(expectedCity, found.Name);

            // should work case insensitive
            found = cities.FindCity("züRicH");
            Assert.AreEqual(expectedCity, found.Name);
        }

        [TestMethod]
        public void TestRoutesReadLinks()
        {
            var cities = new Cities();
            cities.ReadCities(CitiesTestFile);

            var routes = new Routes(cities);

            var count = routes.ReadRoutes(LinksTestFile);
            Assert.AreEqual(7, count);
        }


        [TestMethod]
        public void TestTask2FiredEvents()
        {
            var cities = new Cities();
            cities.ReadCities(CitiesTestFile);

            var routes = new Routes(cities);

            // test available cities
            routes.RouteRequestEvent += TestForCorrectEventArgsWithFoundCities;
            routes.FindShortestRouteBetween("Bern", "Zürich", TransportModes.Rail);

            // test not existing cities
            routes.RouteRequestEvent -= TestForCorrectEventArgsWithFoundCities;
            routes.RouteRequestEvent += TestForCorrectEventArgsWithNotFoundCities;
            routes.FindShortestRouteBetween("doesNotExist", "either", TransportModes.Rail);
        }

        public void TestForCorrectEventArgsWithFoundCities(object sender, RouteRequestEventArgs e)
        {
            Assert.AreEqual("Bern", e.FromCity);
            Assert.AreEqual("Zürich", e.ToCity);
        }

        public void TestForCorrectEventArgsWithNotFoundCities(object sender, RouteRequestEventArgs e)
        {
            Assert.AreEqual("doesNotExist", e.FromCity);
            Assert.AreEqual("either", e.ToCity);
        }

        [TestMethod]
        public void TestTask2EventWithNoObserver()
        {
            var cities = new Cities();
            cities.ReadCities(CitiesTestFile);

            var routes = new Routes(cities);

            // must run without exception
            routes.FindShortestRouteBetween("Bern", "Zürich", TransportModes.Rail);
        }

        [TestMethod]
        public void TestRequestWatcher()
        {
            var reqWatch = new RouteRequestWatcher();

            var cities = new Cities();
            cities.ReadCities(CitiesTestFile);

            var routes = new Routes(cities);

            routes.RouteRequestEvent += reqWatch.LogRouteRequests;

            routes.FindShortestRouteBetween("Bern", "Zürich", TransportModes.Rail);
            routes.FindShortestRouteBetween("Bern", "Zürich", TransportModes.Rail);
            routes.FindShortestRouteBetween("Basel", "Bern", TransportModes.Rail);
            
            Assert.AreEqual(reqWatch.GetCityRequests("Zürich"), 2);
            Assert.AreEqual(reqWatch.GetCityRequests("Bern"), 1);
            Assert.AreEqual(reqWatch.GetCityRequests("Basel"), 0);
        }
    }
}
