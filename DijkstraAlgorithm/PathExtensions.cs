namespace DijkstraAlgorithm;

public static class PathExtensions
{
    public static Path GetPathToNode(this IEnumerable<Path> paths, Node endNode)
    {
        return paths.Single(p => p.EndNode == endNode);
    }
}
