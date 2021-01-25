using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace DijkstraAlgorithm.Tests
{
    public class AlgorithmTests
    {
        [Test]
        public void TestFromAtoBtoCtoD()
        {
            var nodeA = new Node("A");
            var nodeB = new Node("B");
            var nodeC = new Node("C");
            var nodeD = new Node("D");

            nodeA.AddConnection(nodeB, 10);
            nodeB.AddConnection(nodeC, 10);
            nodeC.AddConnection(nodeD, 10);

            var nodes = new List<Node> { nodeA, nodeB, nodeC, nodeD };
            var algorithm = new ShortestPathAlgorithm(nodes);

            var result = algorithm.Run(nodeA);
            result.Should().NotBeNull();
            Print(result);
        }

        [Test]
        public void TestWith8Nodes()
        {
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

            var nodes = new List<Node> {
                node0, node1, node2,
                node3, node4, node5,
                node6, node7, node8
            };
            var algorithm = new ShortestPathAlgorithm(nodes);

            var result = algorithm.Run(node0);
            result.Should().NotBeNull();
            Print(result);
        }

        private static void AddTwoWayConnection(Node nodeA, Node nodeB, double cost)
        {
            nodeA.AddConnection(nodeB, cost);
            nodeB.AddConnection(nodeA, cost);
        }

        private static void Print(ShortestPathsTree result)
        {
            TestContext.Out.WriteLine("----------------");
            TestContext.Out.WriteLine($"Iterations:{result.Iterations}");
            TestContext.Out.WriteLine("Paths found:");
            foreach (var p in result.Paths)
            {
                TestContext.Out.WriteLine(p);
            }
        }
    }
}
