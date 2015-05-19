using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public interface IRoutes
    {
        /// <summary> 
        /// Reads a list of links from the given file. 
        /// Reads only links between existing cities. 
        /// </summary> 
        /// <param name="filename">name of input file</param> 
        /// <returns>number of links read</returns> 
        int ReadRoutes(string filename);

        /// <summary> 
        /// Determines the shortest path between the given cities. 
        /// </summary> 
        /// <param name="fromCity">name of start city</param> 
        /// <param name="toCity">name of destination city</param> 
        /// <param name="mode">transportation mode</param> 
        /// <returns></returns> 
        List<Link> FindShortestRouteBetween(string fromCity, string toCity,
                                              TransportModes mode, IProgress<string> reportProgress); 
    }
}
