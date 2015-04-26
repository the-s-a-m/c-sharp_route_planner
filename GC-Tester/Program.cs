using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace GC_Tester
{
    /* Ergebnisse:
     * Standard GC Zeit:            00:00:00.1297056
     * 
     * Direkt in Config geändert:
     * Disabled GC concurrent:      00:00:00.1223512
     * requires shorter pauses
     * Enabled GC concurrent:       00:00:00.1296547
     * requires higher throughput
     * Enabled GC server:           00:00:00.0741408
     * scale on modern hardware
     * Enabled GC CPU Groups:       00:00:00.1215227
     * works on large datasets
     * Allow very Large Obj:        00:00:00.1267629
     * All config settings enabled: 00:00:00.0745690
     * 
     * Via GCSettings.LatencyMode:
     * GCLatencyMode Batch:         00:00:00.1287325
     * GCLatencyMode Interactive:   00:00:00.1280822
     * cannot tolerate pauses during a certain time window
     * GCLatencyMode LowLatency:    00:00:00.1255356
     * SustainedLowLatency:         00:00:00.1272509
     */
    class Program
    {
        static void Main(string[] args)
        {
            GcTestClass tc = new GcTestClass(1000000);
            //GCSettings.LatencyMode = GCLatencyMode.Batch;
            //GCSettings.LatencyMode = GCLatencyMode.Interactive;
            //GCSettings.LatencyMode = GCLatencyMode.LowLatency;
            //GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;
            tc.RunTest();
            Console.ReadLine();
        }
    }

    public class GcTestClass
    {
        private readonly Stopwatch sw = new Stopwatch();
        private readonly int size;
        public GcTestClass(int size)
        {
            this.size = size;
        }
        

        public void RunTest() 
        {
            List<StringBuilder> l = new List<StringBuilder>();
            for (int i = 0; i < size; i++)
            {
                StringBuilder s = new StringBuilder(100);
                l.Add(s);
            }
            Console.WriteLine(GC.GetTotalMemory(true));
            l = null;
            sw.Start();
            GC.Collect();
            GC.Collect(0, GCCollectionMode.Optimized, false);
            GC.Collect(0, GCCollectionMode.Forced, true);
            sw.Stop();
            Console.WriteLine("Total gc time: {0}", sw.Elapsed);
        }
    }
}
