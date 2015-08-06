using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Lynex.Common.Extension
{
    public static class AssemblyHelper
    {
        public static IEnumerable<Type> GetMapableEnumTypes(this Assembly assembly)
        {
            return assembly.GetTypes().Where(q => q.Namespace == assembly.GetName().Name + ".Enum.Mapable");
        }
    }
}
