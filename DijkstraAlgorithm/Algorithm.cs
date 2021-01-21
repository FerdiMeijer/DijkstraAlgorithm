using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DijkstraAlgorithm
{
    public class Algorithm
    {
        private readonly IEnumerable<Node> _nodes;

        public Algorithm(IEnumerable<Node> nodes)
        {
            this._nodes = nodes ?? throw new ArgumentNullException(nameof(nodes));
        }

        public Result InitializeRoutes(Node startNode)
        {
            // Initialize a list of all nodes with the cost of connecting from 
            // startNode to other nodes set to infinity
            var all = _nodes
                .Where(n => n != startNode)
                .Select(n =>
                {
                    // Find cost of startNode directly connected to other nodes
                    // and overwrite the cost value
                    var cost = startNode.DirectConnectionCost(n);
                    var endpoint = new Connection(n, cost);

                    return new Route(startNode, endpoint);
                })
                .ToList();

            return new Result { StartNode = startNode, Routes = all };
        }

        public Result RelaxNode(Node selectedNode, Result result)
        {
            // Mark EndPoint as Relaxed
            result.Routes.Single(p => p.EndNode == selectedNode).EndNodeRelaxed = true;

            foreach(var connection in selectedNode.Connections)
            {
                RelaxResult(result, selectedNode, connection);
            }

            return result;
        }

        private Result RelaxResult(Result result, Node from, Connection to)
        {
            // Get the Route that has connection as EndNode
            var routeToConnection = result.Routes.Single(r => r.EndNode == to.Node);
            var routeToFrom = result.Routes.Single(r => r.EndNode == from);

            var currentCost = routeToConnection.Cost;
            var costWithAdditionalNode = routeToFrom.Cost.Add(to.Cost);

            // If Cost of the current route to EndNode is greater than
            // the SelectedRoute with the cost of an additional connection to endpoint
            if(currentCost > costWithAdditionalNode)
            {
                routeToConnection.ApplyNewRoute(routeToFrom, to);
            }

            // then add it to the connections of this route

            return result;
        }
    }

    public class Result
    {
        public Node StartNode { get; set; }

        public List<Route> Routes { get; set; }

        public Route ShortestRouteNotYetRelaxed
        {
            get
            {
                var routes = Routes
                    .Where(r => !r.EndNodeRelaxed);
                var min = routes
                    .Select(r => r.Cost)
                    .MinOrDefault();

                return routes.Any() ? routes
                    .FirstOrDefault(p => p.Cost == min): null;
            }
        }
    }
}
