using System.Collections.Generic;
using System.Linq;

namespace DijkstraAlgorithm
{
    public static class ConnectionExtention
    {
        public static double CalculateCost(this IEnumerable<Connection> connections)
        {
            return connections.Any(c => c.Cost == double.PositiveInfinity) ?
                double.PositiveInfinity :
                connections.Select(c => c.Cost).Aggregate((sum, n1) =>
                {
                    return sum.Add(n1);
                });
        }
    }
}