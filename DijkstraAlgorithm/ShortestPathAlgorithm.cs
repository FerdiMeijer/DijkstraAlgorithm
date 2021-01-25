using System;
using System.Collections.Generic;
using System.Linq;

namespace DijkstraAlgorithm
{
    public class ShortestPathAlgorithm
    {
        private readonly IEnumerable<Node> _nodes;
        private ShortestPathsTree _result;

        public ShortestPathAlgorithm(IEnumerable<Node> nodes)
        {
            this._nodes = nodes ?? throw new ArgumentNullException(nameof(nodes));
        }

        public ShortestPathsTree Run(Node startNode)
        {
            _result = InitializeShortestPathTree(startNode);

            var iterations = 0;
            while (_result.TryGetShortestPathNotYetRelaxed(out Path shortestNoneRelaxedPath))
            {
                _result = RelaxNode(shortestNoneRelaxedPath.EndNode);
                iterations++;
            }

            return _result;
        }

        /// <summary>
        /// Initialize a list of node connections and cost,
        /// connecting the start node to all other nodes.
        /// </summary>
        /// <param name="startNode"></param>
        /// <returns></returns>
        private ShortestPathsTree InitializeShortestPathTree(Node startNode)
        {
            var all = _nodes
                .Select(n =>
                {
                    var cost = startNode.GetConnectionCost(n);
                    var endpoint = new Connection(n, cost);

                    return new Path(startNode, endpoint);
                })
                .ToList();

            return new ShortestPathsTree { StartNode = startNode, Paths = all };
        }

        private ShortestPathsTree RelaxNode(Node selectedNode)
        {
            SetPathRelaxed(selectedNode);

            foreach (var connection in selectedNode.ConnectingNodes)
            {
                RelaxResult(_result, selectedNode, connection);
            }

            return _result;
        }

        private void SetPathRelaxed(Node selectedNode)
        {
            _result.Paths.Single(r => r.EndNode == selectedNode).Relax();
        }

        private ShortestPathsTree RelaxResult(ShortestPathsTree result,
            Node from, Connection to)
        {
            var pathToEndPoint = result.Paths.Single(r => r.EndNode == to.Node);
            var pathToFrom = result.Paths.Single(r => r.EndNode == from);

            var currentPathCost = pathToEndPoint.Cost;
            var costWithAdditionalNode = pathToFrom.Cost.Add(to.Cost);

            if (currentPathCost > costWithAdditionalNode)
            {
                pathToEndPoint.Relax(pathToFrom, to);
            }

            return result;
        }
    }
}
