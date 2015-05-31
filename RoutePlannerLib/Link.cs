﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public enum TransportModes { Ship, Rail, Flight, Car, Bus, Tram };

    /// <summary>
    /// Represents a link between two cities with its distance
    /// </summary>
    public class Link : IComparable
    {
        private readonly City fromCity;
        private readonly City toCity;
        readonly double distance;

        readonly TransportModes transportMode = TransportModes.Car;

        public TransportModes TransportMode
        {
            get { return transportMode; }
        }

        public City FromCity
        {
            get { return fromCity; }
        }

        public City ToCity
        {
            get { return toCity; }
        }

        public Link(City fromCity, City toCity, double distance)
        {
            this.fromCity = fromCity;
            this.toCity = toCity;
            this.distance = distance;
        }

        public Link(City fromCity, City toCity, double distance, TransportModes transportMode)
            : this(fromCity, toCity, distance)
        {
            this.transportMode = transportMode;
        }

        public double Distance
        {
            get { return distance; }
        }

        /// <summary>
        /// Specifies "distance" as default compare criteria 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public int CompareTo(object o)
        {
            var other = (Link)o;
            return (int)(1000 * (distance - other.distance));
        }

        /// <summary>
        /// checks if both cities of the link are included in the passed city list
        /// </summary>
        /// <param name="cities">list of city objects</param>
        /// <returns>true if both link-cities are in the list</returns>
        internal bool IsIncludedIn(List<City> cities)
        {
            return cities.Any(c => c.Name == FromCity.Name) &&
                   cities.Any(c => c.Name == ToCity.Name);
        }
    }
}
