using Microsoft.Extensions.Logging;

namespace DijkstraAlgorithm.ConsoleApp;

public class Program
{
    public static void Main(string[] args)
    {
        // Create the 9-node graph from:
        // https://www.geeksforgeeks.org/dijkstras-shortest-path-algorithm-greedy-algo-7/

        var node0 = new Node("0");
        var node1 = new Node("1");
        var node2 = new Node("2");
        var node3 = new Node("3");
        var node4 = new Node("4");
        var node5 = new Node("5");
        var node6 = new Node("6");
        var node7 = new Node("7");
        var node8 = new Node("8");

        // Build the graph with two-way connections
        AddTwoWayConnection(node0, node1, 4);
        AddTwoWayConnection(node0, node7, 8);

        AddTwoWayConnection(node1, node7, 11);
        AddTwoWayConnection(node1, node2, 8);

        AddTwoWayConnection(node2, node5, 4);
        AddTwoWayConnection(node2, node8, 2);
        AddTwoWayConnection(node2, node3, 7);

        AddTwoWayConnection(node3, node5, 14);
        AddTwoWayConnection(node3, node4, 9);

        AddTwoWayConnection(node4, node5, 10);

        AddTwoWayConnection(node5, node6, 2);

        AddTwoWayConnection(node6, node8, 6);
        AddTwoWayConnection(node6, node7, 1);

        AddTwoWayConnection(node7, node8, 7);

        var nodes = new List<Node>
        {
            node0,
            node1,
            node2,
            node3,
            node4,
            node5,
            node6,
            node7,
            node8,
        };

        Console.WriteLine("=== Dijkstra's Shortest Path Algorithm ===");
        Console.WriteLine();
        Console.WriteLine("Graph structure:");
        Console.WriteLine("       4         8");
        Console.WriteLine("  (0)-----(1)-----(2)");
        Console.WriteLine("   |       |     /|\\");
        Console.WriteLine("  8|     11|   7/ | \\4");
        Console.WriteLine("   |       |  /  2|  \\");
        Console.WriteLine("  (7)-----(8)    (5)--(6)");
        Console.WriteLine("   |\\  7     \\  / 2");
        Console.WriteLine("  1| \\        \\/");
        Console.WriteLine("   | 6\\      /10");
        Console.WriteLine("  (6)  \\    /");
        Console.WriteLine("         (4)");
        Console.WriteLine("    \\___9___/");
        Console.WriteLine();

        // Run the algorithm
        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole().SetMinimumLevel(LogLevel.Debug);
        });
        var logger = loggerFactory.CreateLogger<ShortestPathAlgorithm>();

        var algorithm = new ShortestPathAlgorithm(nodes, logger);
        var result = algorithm.Run(node0);

        // Print results
        Console.WriteLine($"Starting node: {node0.Name}");
        Console.WriteLine($"Iterations: {result.Iterations}");
        Console.WriteLine();
        Console.WriteLine("Shortest paths found:");
        Console.WriteLine("----------------------");

        foreach (var path in result.Paths)
        {
            Console.WriteLine(path);
        }
    }

    private static void AddTwoWayConnection(Node nodeA, Node nodeB, double cost)
    {
        nodeA.AddConnection(nodeB, cost);
        nodeB.AddConnection(nodeA, cost);
    }
}
