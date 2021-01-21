using System;
using System.Collections.Generic;

namespace DijkstraAlgorithm
{
    public class Connection
    {
        public Connection(Node node, double cost)
        {
            Cost = cost;
            Node = node;
        }
        public override string ToString()
        {
            return $"[{Node.Name}:{Cost}]";
        }

        public double Cost { get; }

        public Node Node { get; }
    }
}
