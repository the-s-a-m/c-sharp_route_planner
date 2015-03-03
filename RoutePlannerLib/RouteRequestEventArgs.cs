using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class RouteRequestEventArgs : System.EventArgs
    {

        public TransportModes TransportMode { get; private set; }
        public string FromCity { get; private set; }
        public string ToCity { get; private set; }


        public RouteRequestEventArgs(string _fromCity, string _toCity, TransportModes _transportMode)
        {
            FromCity = _fromCity;
            ToCity = _toCity;
            TransportMode = _transportMode;
        }
    }
}
