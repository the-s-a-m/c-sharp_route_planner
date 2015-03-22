using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util
{
    public class SimpleObjectReader
    {
        public StringReader stream { get; private set; }

        public SimpleObjectReader(StringReader stream)
        {
            this.stream = stream;
        }

        public Object Next()
        {
            if(stream != null) 
            {
                var s = stream.ReadLine();
                if(s.Contains("Instance of "))
                {
                    var splits = s.Split(' ');                 
                    var o = Activator.CreateInstance(Type.GetType(splits[2]));
                    var t = Type.GetType(splits[2]);
                    
                    while (s != null && o != null && !s.Contains("End of instance"))
                    {
                        String[] stringSplit;
                        PropertyInfo pinfo;
                        if (s.Contains("is a nested object"))
                        {
                            stringSplit = s.Split(' ');
                            pinfo = o.GetType().GetProperty(stringSplit[0]);
                            Console.WriteLine(stringSplit[0]);
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
                                        pinfo.SetValue(o, Double.Parse(stringSplit[1]));
                                    }
                                    else if (pinfo.GetValue(o) is Int32)
                                    {
                                        pinfo.SetValue(o, Int32.Parse(stringSplit[1]));
                                    }
                                }
                            }
                        }
                        s = stream.ReadLine();
                    }
                    return o;
                }
            }
            return null;
        }

    }
}
