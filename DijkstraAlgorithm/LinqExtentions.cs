using System.Collections.Generic;
using System.Linq;

namespace DijkstraAlgorithm
{
    public static class LinqExtentions
    {
        public static T MinOrDefault<T>(this IEnumerable<T> sequence)
        {
            if (sequence.Any())
            {
                return sequence.Min();
            }
            else
            {
                return default(T);
            }
        }

        public static bool IsEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable?.Any() != true;
        }
    }
}