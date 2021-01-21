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
    }
}