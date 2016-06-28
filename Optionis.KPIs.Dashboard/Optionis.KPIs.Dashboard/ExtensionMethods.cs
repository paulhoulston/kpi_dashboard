using System.Collections.Generic;

namespace Optionis.KPIs.Dashboard
{
    static class ExtensionMethods
    {
        public static IEnumerable<T> NullSafe<T>(this IEnumerable<T> enumerable)
        {
            return enumerable ?? new T[] { };
        }
    }
}