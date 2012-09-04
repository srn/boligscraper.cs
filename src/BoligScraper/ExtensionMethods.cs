using System;
using System.Collections.Generic;

namespace BoligScraper
{
    public static class ExtensionMethods
    {
        public static int ForEach<T>(this IEnumerable<T> list, Action<int, T> callback)
        {
            if (callback == null)
                throw new ArgumentNullException("callback");

            int index = 0;

            foreach (var elem in list)
                callback(index++, elem);

            return index;
        }
    }
}
