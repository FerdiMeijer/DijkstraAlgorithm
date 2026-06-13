namespace DijkstraAlgorithm;

/// <summary>
/// A list of edges from a start node to the end node
/// </summary>
public class Path(Node startNode, Edge end)
{
    private List<Edge> _edges = [end];
    private bool _isRelaxed;

    public Node EndNode { get; } = end.Node;

    public Node StartNode { get; } =
        startNode ?? throw new ArgumentNullException(nameof(startNode));

    public IEnumerable<Edge> Edges => _edges;

    public bool IsRelaxed => _isRelaxed;

    public double Cost => _edges.CalculateCost();

    public void Update(Path path, Edge end)
    {
        if (!path.IsRelaxed)
        {
            throw new ArgumentException("Path must be relaxed before updating.");
        }

        if (end.Node != EndNode)
        {
            throw new ArgumentException("Endpoint node must match the path's end node.");
        }

        _edges = [.. path.Edges, end];
    }

    public void Relax()
    {
        if (_isRelaxed)
        {
            throw new InvalidOperationException("Path already relaxed!");
        }

        _isRelaxed = true;
    }

    public override string ToString()
    {
        var cost = Cost == double.PositiveInfinity ? "∞" : Cost.ToString();
        return $"{StartNode.Name}=>{_edges.Select(c => $"{c.Node.Name}:{c.Cost}").ToCsv("=>")} ∑: {cost}";
    }
}
