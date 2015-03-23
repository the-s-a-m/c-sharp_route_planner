using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerTest
{
    [TestClass]
    [DeploymentItem("data/citiesTestDataLab3.txt")]
    [DeploymentItem("data/linksTestDataLab3.txt")]
    public class Lab06Test
    {
        [TestMethod]
        public void TestFindCitiesByTransportMode()
        {
            Cities cities = new Cities();
            cities.ReadCities(@"citiesTestDataLab3.txt");
            var routes = new Routes(cities);
            routes.ReadRoutes(@"linksTestDataLab3.txt");

            City[] citiesByMode = routes.FindCities(TransportModes.Rail);

            Assert.AreEqual(11, citiesByMode.Count());

            // there must be no cities
            City[] emptyCitiesByMode = routes.FindCities(TransportModes.Bus);
            Assert.AreEqual(0, emptyCitiesByMode.Count());

        }
    }
}