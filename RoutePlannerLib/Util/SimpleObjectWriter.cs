using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Reflection;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util
{
    public class SimpleObjectWriter
    {
        public StringWriter Stream { get; private set; }

        public SimpleObjectWriter(StringWriter stream)
        {
            this.Stream = stream;
        }

        public void Next(object c)
        {
            var getValue = new Func<PropertyInfo, object, object>((p,obj) => p.GetValue(obj, System.Reflection.BindingFlags.GetProperty, null, null, CultureInfo.InvariantCulture));
            if (Stream != null && c != null)
            {
                Stream.WriteLine("Instance of {0}", c.GetType().FullName);
                foreach(var p in c.GetType().GetProperties())
                {
                    var typeOfProp = p.GetValue(c);
                    if (!p.CustomAttributes.Select(x => x.AttributeType == typeof(XmlIgnoreAttribute)).Any())
                    {
                        if (typeOfProp is string)
                        {
                            Stream.WriteLine("{0}=\"{1}\"", p.Name, getValue(p, c));
                        }
                        else if (typeOfProp is ValueType)
                        {
                            Stream.WriteLine("{0}={1}", p.Name, getValue(p, c));
                        }
                        else
                        {
                            Stream.WriteLine("{0} is a nested object...", p.Name);
                            this.Next(getValue(p, c));
                        }
                    }
                }
                Stream.WriteLine("End of instance");
            }
        }
    }
}
