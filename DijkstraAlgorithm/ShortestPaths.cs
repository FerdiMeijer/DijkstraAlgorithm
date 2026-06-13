namespace DijkstraAlgorithm;

public class ShortestPaths
{
    private readonly List<Path> _paths = [];

    public int Iterations { get; set; }

    public required Node StartNode { get; init; }

    public required IEnumerable<Path> Paths
    {
        get => _paths;
        init => _paths = value.ToList();
    }
}
