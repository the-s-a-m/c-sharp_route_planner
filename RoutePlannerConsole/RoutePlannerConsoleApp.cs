using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerConsole
{
    class RoutePlannerConsoleApp
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to RoutePlanner (Version " +
                System.Reflection.Assembly.GetExecutingAssembly().GetName().Version + 
                ") ");
            var wayPoint = new WayPoint("Windisch", 47.479319847061966, 8.212966918945312);
            Console.WriteLine("{0}: {1}/{2}", wayPoint.Name, wayPoint.Latitude, wayPoint.Longitude);
            Console.WriteLine(wayPoint.ToString());
            var bern = new WayPoint("Bern", 46.949690, 7.442420);
            var tripolis = new WayPoint("Tripolis", 32.815062, 13.105445);
            Console.WriteLine(bern.Distance(tripolis));
            Console.ReadLine();
        }
    }
}
