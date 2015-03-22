using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib;
using System.IO;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util;
using System;


namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerTest
{
    [TestClass]
    public class Lab5DeSerializerTest
    {
        [TestMethod]
        public void TestSerializeSingleCityWithValues()
        {
            var c = new City("Aarau", "Switzerland", 10, 1.1, 2.2);

            var stream = new StringWriter();
            var writer = new SimpleObjectWriter(stream);
            writer.Next(c);
            var result = stream.ToString();

            Assert.AreEqual(CityWithValues, result);
        }

        private const string CityWithValues = "Instance of Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.City\r\nName=\"Aarau\"\r\nCountry=\"Switzerland\"\r\nPopulation=10\r\nLocation is a nested object...\r\nInstance of Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.WayPoint\r\nName=\"Aarau\"\r\nLongitude=2.2\r\nLatitude=1.1\r\nEnd of instance\r\nEnd of instance\r\n";

        [TestMethod]
        public void TestSerializeMultCitiesWithValues()
        {
            const string expectedString1 = "Instance of Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.City\r\nName=\"Aarau\"\r\nCountry=\"Switzerland\"\r\nPopulation=10\r\nLocation is a nested object...\r\nInstance of Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.WayPoint\r\nName=\"Aarau\"\r\nLongitude=2.2\r\nLatitude=1.1\r\nEnd of instance\r\nEnd of instance\r\n";
            const string expectedString2 = "Instance of Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.City\r\nName=\"Bern\"\r\nCountry=\"Switzerland\"\r\nPopulation=10\r\nLocation is a nested object...\r\nInstance of Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.WayPoint\r\nName=\"Bern\"\r\nLongitude=2.2\r\nLatitude=1.1\r\nEnd of instance\r\nEnd of instance\r\n";
            const string expectedString = expectedString1 + expectedString2;
            var c1 = new City("Aarau", "Switzerland", 10, 1.1, 2.2);
            var c2 = new City("Bern", "Switzerland", 10, 1.1, 2.2);

            var stream = new StringWriter();
            var writer = new SimpleObjectWriter(stream);
            writer.Next(c1);
            var result = stream.ToString();
            Assert.AreEqual(expectedString1, result);

            // write second city
            writer.Next(c2);

            // result is expected to contain both cities
            result = stream.ToString();
            Assert.AreEqual(expectedString, result);
        }



        [TestMethod]
        public void TestDeserializeSingleCityWithValues()
        {
            var expectedCity = new City("Aarau", "Switzerland", 10, 1.1, 2.2);
            var stream = new StringReader(CityWithValues);
            var reader = new SimpleObjectReader(stream);
            var city = reader.Next() as City;
            Assert.IsNotNull(city);
            Assert.AreEqual(expectedCity.Name, city.Name);
            Assert.AreEqual(expectedCity.Country, city.Country);
            Assert.AreEqual(expectedCity.Location.Latitude, city.Location.Latitude);
        }

        [TestMethod]
        public void TestDeserializeMultCitiesWithValues()
        {
            const string cityString1 = "Instance of Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.City\r\nName=\"Aarau\"\r\nCountry=\"Switzerland\"\r\nPopulation=10\r\nLocation is a nested object...\r\nInstance of Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.WayPoint\r\nName=\"Aarau\"\r\nLongitude=2.2\r\nLatitude=1.1\r\nEnd of instance\r\nEnd of instance\r\n";
            const string cityString2 = "Instance of Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.City\r\nName=\"Bern\"\r\nCountry=\"Switzerland\"\r\nPopulation=10\r\nLocation is a nested object...\r\nInstance of Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.WayPoint\r\nName=\"Bern\"\r\nLongitude=2.2\r\nLatitude=1.1\r\nEnd of instance\r\nEnd of instance\r\n";
            const string cityString = cityString1 + cityString2;
            var expectedCity1 = new City("Aarau", "Switzerland", 10, 1.1, 2.2);
            var expectedCity2 = new City("Bern", "Switzerland", 10, 1.1, 2.2);
            var stream = new StringReader(cityString);
            var reader = new SimpleObjectReader(stream);
            var city1 = reader.Next() as City;

            Assert.IsNotNull(city1);
            Assert.AreEqual(expectedCity1.Name, city1.Name);
            Assert.AreEqual(expectedCity1.Country, city1.Country);
            Assert.AreEqual(expectedCity1.Location.Latitude, city1.Location.Latitude);
            var city2 = reader.Next() as City;
            Assert.IsNotNull(city2);
            Assert.AreEqual(expectedCity2.Name, city2.Name);
            Assert.AreEqual(expectedCity2.Country, city2.Country);
            Assert.AreEqual(expectedCity2.Location.Latitude, city2.Location.Latitude);
        }
    }
    
}
