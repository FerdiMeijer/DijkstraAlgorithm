namespace DijkstraAlgorithm;

/// <summary>
/// A list of edges from a start node to the end node
/// </summary>
public class Path(Node startNode, Edge endpoint)
{
    private List<Edge> _edges = [endpoint];
    private bool _isRelaxed;

    public Node EndNode { get; } = endpoint.Node;

    public Node StartNode { get; } =
        startNode ?? throw new ArgumentNullException(nameof(startNode));

    public IEnumerable<Edge> Edges => _edges;

    public bool IsRelaxed => _isRelaxed;

    public double Cost => _edges.CalculateCost();

    public void Relax(Path pathToEndNode, Edge toEndpoint)
    {
        if (toEndpoint.Node != EndNode)
        {
            throw new ArgumentException("Endpoint node must match the path's end node.");
        }

        _edges = [.. pathToEndNode.Edges, toEndpoint];
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
