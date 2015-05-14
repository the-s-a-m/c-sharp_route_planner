using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util
{
    public class SimpleObjectWriter
    {
        public StringWriter Stream { get; private set; }

        public SimpleObjectWriter(StringWriter stream)
        {
            this.Stream = stream;
        }

        public void Next(Object c)
        {
            if (Stream != null && c != null)
            {
                var cultInv = CultureInfo.InvariantCulture;
                Stream.Write("Instance of " + c.GetType().FullName + "\r\n", cultInv);
                foreach(var p in c.GetType().GetProperties())
                {
                    var typeOfProp = p.GetValue(c);
                    if (p.CustomAttributes.Select(x => x.AttributeType == typeof(XmlIgnoreAttribute)).Count() != 1)
                    {
                        if (typeOfProp is string)
                        {
                            Stream.Write(p.Name + "=\"" + p.GetValue(c) + "\"\r\n", cultInv);
                        }
                        else if (typeOfProp is ValueType)
                        {
                            Stream.Write(p.Name + "=" + p.GetValue(c) + "\r\n", cultInv);
                        }
                        else
                        {
                            Stream.Write(p.Name + " is a nested object...\r\n", cultInv);
                            this.Next(p.GetValue(c));
                        }
                    }
                }
                Stream.Write("End of instance\r\n", cultInv);
            }
        }
    }
}
