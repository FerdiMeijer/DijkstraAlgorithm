using System;
using System.Collections.Generic;
using System.Linq;

namespace DijkstraAlgorithm
{
    public class Node
    {
        private List<Connection> _connections;

        public Node(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            _connections = new List<Connection>();
        }

        public double DirectConnectionCost(Node n)
        {
            return this.Connections
                .FirstOrDefault(c => c.Node == n)?.Cost ?? Double.PositiveInfinity;
        }

        public override string ToString()
        {
            return $"{Name}";
        }

        public string Name { get; }

        public void AddConnection(Node node, double cost)
        {
            if (node == this)
            {
                throw new ArgumentException("Should not add node to self.");
            }

            _connections.Add(new Connection(node, cost));
        }

        public IEnumerable<Connection> Connections
        {
            get { return _connections; }
        }
    }
}