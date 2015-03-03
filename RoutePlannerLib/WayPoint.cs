using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class WayPoint
    {
        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        private const int ROfEarth = 6371;

        public WayPoint(string _name, double _laltitude, double _longitude)
        {
            Name = _name;
            Latitude = _laltitude;
            Longitude = _longitude;
        }

        public override String ToString() 
        {
            if(Name == null) {
                return "WayPoint: " + Math.Round(Latitude, 2) + "/" + Math.Round(Longitude, 2);
            }
            else
            {
                return "WayPoint: " + Name + " " + Math.Round(Latitude, 2) + "/" +  Math.Round(Longitude, 2);
            }
        }

        public double Distance(WayPoint target)
        {
            double thisLatAsRad = DegToRad(this.Latitude);
            double targetLatAsRad = DegToRad(target.Latitude);
            return ROfEarth * Math.Acos((Math.Sin(thisLatAsRad) * Math.Sin(targetLatAsRad) + Math.Cos(thisLatAsRad)
                * Math.Cos(targetLatAsRad) * Math.Cos(DegToRad(this.Longitude) - DegToRad(target.Longitude))));
        }
        public double DegToRad(double angle)
        {
            return (Math.PI / 180) * angle;
        }

    }
}
