using System;
using System.Collections.Generic;

namespace Optionis.KPIs.Dashboard.Application
{
    public static class ExtensionMethods
    {
        public static void ForEachNullSafe<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in (enumerable ?? new T[]{})) {
                action (item);
            }
        }
    }
}