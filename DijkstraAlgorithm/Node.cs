using System;
using System.Collections.Generic;
using System.Linq;

namespace DijkstraAlgorithm
{
    public class Node
    {
        private List<Connection> _connectingNodes;

        public Node(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            _connectingNodes = new List<Connection>();
        }

        /// <summary>
        /// If there is no direct connection from this node to the other node
        /// the cost will be set to infinite.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public double GetConnectionCost(Node node)
        {
            if(this == node)
            {
                return 0;
            }

            return this.ConnectingNodes
                .FirstOrDefault(c => c.Node == node)?.Cost ?? Double.PositiveInfinity;
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
                throw new ArgumentException("Node should not be connected to self.");
            }

            _connectingNodes.Add(new Connection(node, cost));
        }

        public IEnumerable<Connection> ConnectingNodes
        {
            get { return _connectingNodes; }
        }
    }
}