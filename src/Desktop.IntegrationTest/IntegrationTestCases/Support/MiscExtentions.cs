using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDS.WITSMLstudio.Desktop.IntegrationTestCases.Support
{
    public static class MiscExtentions
    {

        /// <summary>
        /// Take last N items from source list
        /// </summary>
        /// <param name="source">source list</param>
        /// <param name="N">number of item to take (try to make it greater than list count)</param>
        /// <returns>Returns a new list that have last N items of source list.</returns>
        public static IList<T> TakeLast<T>(this IList<T> source, int N)
        {
            List<T> lastItems = new List<T>();

            for (int i = Math.Max(0, source.Count - N); i < source.Count; ++i)
            {
                lastItems.Add(source[i]);
            }
            return lastItems;
        }
    }
}
