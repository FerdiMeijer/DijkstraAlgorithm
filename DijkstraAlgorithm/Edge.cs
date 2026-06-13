namespace DijkstraAlgorithm;

public class Edge(Node connectedTo, double cost)
{
    public double Cost { get; } = cost;

    public Node Node { get; } = connectedTo;

    public override string ToString() => $"[{Node.Name}:{Cost}]";
}
