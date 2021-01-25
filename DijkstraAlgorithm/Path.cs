using System;
using System.Collections.Generic;
using System.Linq;

namespace DijkstraAlgorithm
{
    /// <summary>
    /// A list of connections from a start node to the end node
    /// </summary>
    public class Path
    {
        public Node EndNode { get; }

        private List<Connection> _connections;
        private bool _isRelaxed;

        public Path(Node startNode, Connection endpoint)
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

        public void Relax(Path pathToEndNode, Connection toEndpoint)
        {
            if (toEndpoint.Node != EndNode)
            {
                throw new ArgumentException();
            }

            _connections = pathToEndNode.Connections
                .Concat(new List<Connection> { toEndpoint })
                .ToList();
        }
        public void Relax()
        {
            if(_isRelaxed)
            {
                throw new Exception("Path already relaxed!");
            }

            _isRelaxed = true;
        }

        public bool IsRelaxed => _isRelaxed;
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