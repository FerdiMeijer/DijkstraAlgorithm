namespace DijkstraAlgorithm;

public class Edge(Node node, double cost)
{
    public double Cost { get; } = cost;

    public Node Node { get; } = node;

    public override string ToString() => $"[{Node.Name}:{Cost}]";
}
