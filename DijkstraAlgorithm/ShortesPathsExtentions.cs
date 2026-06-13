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

        path = result.Paths.Where(p => !p.IsRelaxed).MinBy(p => p.Cost)!;

        return path != null;
    }

    /// <summary>
    /// Relaxes a node by first marking it as finalized, then examining all its
    /// outgoing edges and updating the shortest paths to neighboring nodes
    /// if a shorter path is found through this node.
    /// </summary>
    public static void RelaxPath(this ShortestPaths result, Path path)
    {
        // Mark the path to this node as relaxed FIRST
        // (its shortest path is now finalized)
        path.Relax();

        // Then update neighbors using the now-relaxed path
        foreach (var edge in path.EndNode.Edges)
        {
            var currentShortestPath = result.Paths.GetPathToNode(edge.Node);
            if (currentShortestPath.IsRelaxed)
            {
                continue;
            }

            var currentCost = currentShortestPath.Cost;
            var costViaNode = path.Cost.Add(edge.Cost);

            if (currentCost > costViaNode)
            {
                currentShortestPath.Update(path, edge);
            }
        }
    }
}
