namespace DijkstraAlgorithm;

public class Node(string name)
{
    private readonly List<Edge> _edges = [];

    public string Name { get; } = name ?? throw new ArgumentNullException(nameof(name));

    public IEnumerable<Edge> Edges => _edges;

    /// <summary>
    /// Get the cost of the connection from this node to the other node.
    /// If there is no direct connection from this node to the other node
    /// the cost will be set to infinite.
    /// </summary>
    public double GetConnectionCost(Node node)
    {
        if (this == node)
        {
            return 0;
        }

        return _edges.FirstOrDefault(c => c.Node == node)?.Cost ?? double.PositiveInfinity;
    }

    public void AddConnection(Node node, double cost)
    {
        if (node == this)
        {
            throw new ArgumentException("Node should not be connected to self.");
        }

        _edges.Add(new Edge(node, cost));
    }

    public override string ToString() => $"{Name}";
}
