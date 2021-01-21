using System;
using System.Collections.Generic;
using System.Linq;

namespace DijkstraAlgorithm
{
    public class Route
    {
        public Node EndNode { get; }

        private List<Connection> _connections;

        public Route(Node startNode, Connection endpoint)
        {
            EndNode = endpoint.Node;
            StartNode = startNode ?? throw new System.ArgumentNullException(nameof(startNode));

            _connections = new List<Connection> { endpoint };
        }

        public IEnumerable<Connection> Connections 
        {
            get
            { 
                return _connections;
            }
        }

        public void ApplyNewRoute(Route intermediateRoute, Connection toEndpoint)
        {
            if (toEndpoint.Node != EndNode)
            {
                throw new ArgumentException();
            }

            _connections = intermediateRoute.Connections
                .Concat(new List<Connection> { toEndpoint })
                .ToList();
        }

        public bool EndNodeRelaxed { get; set; }
        public Node StartNode { get; }

        public double Cost
        {
            get
            {
                return _connections.CalculateCost();
            }
        }

        public override string ToString()
        {
            var cost = Cost == double.PositiveInfinity ? "∞" : Cost.ToString();
            return $"{StartNode.Name}=>{_connections.Select(c => $"{c.Node.Name}:{c.Cost}").ToCsv("=>")} ∑: {cost}";
        }
    }
}