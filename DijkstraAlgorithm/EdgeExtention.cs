namespace DijkstraAlgorithm;

public static class EdgeExtension
{
    public static double CalculateCost(this IEnumerable<Edge> edges)
    {
        return edges.Any(c => c.Cost == double.PositiveInfinity)
            ? double.PositiveInfinity
            : edges.Select(c => c.Cost).Aggregate((sum, n1) => sum.Add(n1));
    }
}
