using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util
{
    public class SimpleObjectWriter
    {
        public StringWriter stream { get; private set; }

        public SimpleObjectWriter(StringWriter stream)
        {
            this.stream = stream;
        }

        public void Next(Object c)
        {
            if (stream != null && c != null)
            {
                stream.Write("Instance of " + c.GetType().FullName + "\r\n");
                foreach(var p in c.GetType().GetProperties()) 
                {
                    var typeOfProp = p.GetValue(c);
                    if (typeOfProp is string)
                    {
                        stream.Write(p.Name + "=\"" + p.GetValue(c) + "\"\r\n");
                    }
                    else if (typeOfProp is System.ValueType)
                    {
                        stream.Write(p.Name + "=" + p.GetValue(c) + "\r\n");
                    }
                    else
                    {
                        stream.Write(p.Name + " is a nested object...\r\n");
                        this.Next(p.GetValue(c));
                    }
                }
                stream.Write("End of instance\r\n");
            }
        }
    }
}
