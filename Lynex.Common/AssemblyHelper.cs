using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Lynex.Common.Model;

namespace Lynex.Common
{
    public static class AssemblyHelper
    {
        public static IEnumerable<Type> GetMapableEnumTypes(this Assembly assembly)
        {
            return assembly.GetTypes().Where(q => q.Namespace == assembly.GetName().Name + ".Enum.Mapable");
        }
    }
}
