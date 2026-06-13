namespace DijkstraAlgorithm;

public class Node
{
    private readonly List<Edge> _connectedEdges = [];

    public Node(string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public string Name { get; }

    public IEnumerable<Edge> ConnectedEdges => _connectedEdges;

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

        return ConnectedEdges.FirstOrDefault(c => c.Node == node)?.Cost ?? double.PositiveInfinity;
    }

    public void AddConnection(Node node, double cost)
    {
        if (node == this)
        {
            throw new ArgumentException("Node should not be connected to self.");
        }

        _connectedEdges.Add(new Edge(node, cost));
    }

    public override string ToString() => $"{Name}";
}
