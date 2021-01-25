using System.Linq;

namespace DijkstraAlgorithm
{
    public static class ShortesPathTreeExtentions
    {
        public static bool TryGetShortestPathNotYetRelaxed(this ShortestPathsTree result, out Path path)
        {
            if (result == null || result.Paths.IsEmpty())
            {
                path = default;

                return false;
            }

            var paths = result.Paths
                .Where(r => !r.IsRelaxed);
            var min = paths
                .Select(r => r.Cost)
                .MinOrDefault();

            path = paths.FirstOrDefault(p => p.Cost == min);

            return path != null;
        }
    }
}