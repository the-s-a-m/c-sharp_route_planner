using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Dynamic
{
    public class World : DynamicObject
    {
        private Cities cities;

        public World(Cities c)
        {
            cities = c;

        }
        public override bool TryInvokeMember(
            InvokeMemberBinder binder, object[] args,
            out object result)
        {
            var c = cities.FindCity(binder.Name);
            if (c == null)
            {
                result = "The city \"" + binder.Name + "\" does not exist!";
                return true;
            }
            else
            {
                result = c;
                return true;
            }
        }


    }
}
