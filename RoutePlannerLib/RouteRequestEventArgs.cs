using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class RouteRequestEventArgs : EventArgs
    {

        public TransportModes TransportMode { get; private set; }
        public string FromCity { get; private set; }
        public string ToCity { get; private set; }


        public RouteRequestEventArgs(string fromCity, string toCity, TransportModes transportMode)
        {
            FromCity = fromCity;
            ToCity = toCity;
            TransportMode = transportMode;
        }
    }
}
