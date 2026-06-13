using Microsoft.Extensions.Logging;

namespace DijkstraAlgorithm;

public class ShortestPathAlgorithm
{
    private readonly IEnumerable<Node> _nodes;
    private readonly ILogger<ShortestPathAlgorithm> _logger;
    private ShortestPaths _result = null!;

    public ShortestPathAlgorithm(IEnumerable<Node> nodes, ILogger<ShortestPathAlgorithm> logger)
    {
        _nodes = nodes ?? throw new ArgumentNullException(nameof(nodes));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public ShortestPaths Run(Node startNode)
    {
        _logger.LogDebug("Starting shortest path algorithm from node {NodeName}", startNode.Name);

        _result = InitializeShortestPathTree(startNode);

        var iterations = 0;
        while (_result.TryGetShortestPathNotYetRelaxed(out var shortestNoneRelaxedPath))
        {
            _logger.LogDebug(
                "Shortest non-relaxed path found to node {NodeName} with cost {Cost}",
                shortestNoneRelaxedPath.EndNode.Name,
                shortestNoneRelaxedPath.Cost
            );

            iterations++;
            _logger.LogDebug(
                "Iteration {Count}: relaxing node {NodeName}",
                iterations,
                shortestNoneRelaxedPath.EndNode.Name
            );
            _result = RelaxNode(shortestNoneRelaxedPath.EndNode);
        }

        _result.Iterations = iterations;

        _logger.LogDebug("Algorithm completed in {Iterations} iterations", iterations);

        return _result;
    }

    /// <summary>
    /// Initialize a list of node edges and cost,
    /// connecting the start node to all other nodes.
    /// </summary>
    private ShortestPaths InitializeShortestPathTree(Node startNode)
    {
        var paths = _nodes
            .Select(n =>
            {
                var cost = startNode.GetConnectionCost(n);
                var endpoint = new Edge(n, cost);
                return new Path(startNode, endpoint);
            })
            .ToList();

        return new ShortestPaths { StartNode = startNode, Paths = paths };
    }

    /// <summary>
    /// Relaxes a node by examining all its outgoing edges and updating the shortest
    /// paths to neighboring nodes if a shorter path is found through this node.
    /// After processing all edges, marks the node as relaxed (finalized).
    /// </summary>
    private ShortestPaths RelaxNode(Node node)
    {
        foreach (var edge in node.ConnectedEdges)
        {
            _logger.LogDebug(
                "Examining edge from {FromNode} to {ToNode} with cost {Cost}",
                node.Name,
                edge.Node.Name,
                edge.Cost
            );

            RelaxResult(_result, node, edge);
        }

        SetPathRelaxed(node);

        return _result;
    }

    /// <summary>
    /// Marks the path to the specified node as relaxed (finalized), indicating
    /// that the shortest path to this node has been determined and will not change.
    /// </summary>
    private void SetPathRelaxed(Node selectedNode)
    {
        _result.Paths.Single(r => r.EndNode == selectedNode).Relax();
    }

    /// <summary>
    /// Checks if going through the 'from' node to reach the 'to' edge's destination
    /// is cheaper than the current known path. If so, updates the path with the
    /// new shorter route and lower cost.
    /// </summary>
    private ShortestPaths RelaxResult(ShortestPaths result, Node from, Edge to)
    {
        var pathToTo = result.Paths.Single(r => r.EndNode == to.Node);
        var pathToFrom = result.Paths.Single(r => r.EndNode == from);

        var currentPathCost = pathToTo.Cost;
        var costWithAdditionalNode = pathToFrom.Cost.Add(to.Cost);

        if (currentPathCost > costWithAdditionalNode)
        {
            _logger.LogDebug(
                "Found shorter path to {Node}: {OldCost} -> {NewCost}",
                to.Node.Name,
                currentPathCost,
                costWithAdditionalNode
            );
            pathToTo.Relax(pathToFrom, to);
        }

        return result;
    }
}
