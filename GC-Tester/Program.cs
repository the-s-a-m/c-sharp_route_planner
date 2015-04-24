using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GC_Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            new BigClass(5000);
        }
    }

    public class BigClass
    {
        public BigClass(int size)
        {
            for (int i = 0; i < size; i++)
            {
                new Object();
            }
        }
    }
}
