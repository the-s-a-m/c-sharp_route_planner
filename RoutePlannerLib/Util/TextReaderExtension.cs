﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util
{
    public static class TextReaderExtension
    {
       
        public static IEnumerable<string[]> GetSplittedLines(this TextReader tr, char breakChar)
        {
            String c = tr.ReadLine();
            while (c != null)
            {
                yield return c.Split(breakChar);
                c = tr.ReadLine();
            }
        }

        public static IEnumerable<T> ForEach2<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (var item in enumeration)
            {
                action(item);
                yield return item;
            }
        }
    }
}
