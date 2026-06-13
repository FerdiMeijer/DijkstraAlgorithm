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

        while (_result.TryGetShortestPathNotYetRelaxed(out var shortestNoneRelaxedPath))
        {
            _logger.LogDebug(
                "Shortest non-relaxed path found to node {NodeName} with cost {Cost}",
                shortestNoneRelaxedPath.EndNode.Name,
                shortestNoneRelaxedPath.Cost
            );

            _result.Iterations++;
            _logger.LogDebug(
                "Iteration {Count}: relaxing node {NodeName}",
                _result.Iterations,
                shortestNoneRelaxedPath.EndNode.Name
            );

            _result.RelaxPath(shortestNoneRelaxedPath);
        }

        _logger.LogDebug("Algorithm completed in {Iterations} iterations", _result.Iterations);

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
}
