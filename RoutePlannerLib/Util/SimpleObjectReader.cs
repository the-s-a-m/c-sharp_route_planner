using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util
{
    public class SimpleObjectReader
    {
        public StringReader Stream { get; private set; }

        public SimpleObjectReader(StringReader stream)
        {
            this.Stream = stream;
        }

        public Object Next()
        {
            if(Stream != null) 
            {
                var s = Stream.ReadLine();
                if(s != null && s.Contains("Instance of "))
                {
                    var splits = s.Split(' ');
                    var t = Type.GetType(splits[2]);
                    if (t == null) { return null;}
                    var o = Activator.CreateInstance(t);
                    
                    while (s != null && o != null && !s.Contains("End of instance"))
                    {
                        String[] stringSplit;
                        PropertyInfo pinfo;
                        if (s.Contains("is a nested object"))
                        {
                            stringSplit = s.Split(' ');
                            pinfo = o.GetType().GetProperty(stringSplit[0]);
                            //Console.WriteLine(stringSplit[0]);
                            pinfo.SetValue(o, this.Next());
                        }
                        else
                        {
                            stringSplit = s.Split('=');
                            pinfo = o.GetType().GetProperty(stringSplit[0]);
                            if (pinfo != null)
                            {
                                if (pinfo.GetValue(o) is string)
                                {
                                    pinfo.SetValue(o, stringSplit[1].Trim('\"'));
                                }
                                else if (pinfo.GetValue(o) is System.ValueType)
                                {
                                    if (pinfo.GetValue(o) is double)
                                    {
                                        pinfo.SetValue(o, Double.Parse(stringSplit[1], CultureInfo.InvariantCulture));
                                    }
                                    else if (pinfo.GetValue(o) is Int32)
                                    {
                                        pinfo.SetValue(o, Int32.Parse(stringSplit[1], CultureInfo.InvariantCulture));
                                    }
                                }
                            }
                        }
                        s = Stream.ReadLine();
                    }
                    return o;
                }
            }
            return null;
        }

    }
}
