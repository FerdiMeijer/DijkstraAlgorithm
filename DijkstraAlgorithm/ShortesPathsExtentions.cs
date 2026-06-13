namespace DijkstraAlgorithm;

public static class ShortestPathsExtensions
{
    public static bool TryGetShortestPathNotYetRelaxed(this ShortestPaths result, out Path path)
    {
        if (result == null || result.Paths.IsEmpty())
        {
            path = default!;
            return false;
        }

        var paths = result.Paths.Where(r => !r.IsRelaxed);
        var min = paths.Select(p => p.Cost).MinOrDefault();

        path = paths.FirstOrDefault(p => p.Cost == min)!;

        return path != null;
    }
}
