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
        public void Test()
        {
            var nodeA = new Node("A");
            var nodeB = new Node("B");
            var nodeC = new Node("C");
            var nodeD = new Node("D");

            nodeA.AddConnection(nodeB, 10);
            nodeB.AddConnection(nodeC, 10);
            nodeC.AddConnection(nodeD, 10);

            var nodes = new List<Node> { nodeA, nodeB, nodeC, nodeD };
            var algorithm = new Algorithm(nodes);
            var result = algorithm.InitializeRoutes(nodeA);
            Print(result);

            var pathB = result.ShortestRouteNotYetRelaxed;
            var newResult = algorithm.RelaxNode(pathB.EndNode, result);
            Print(newResult);

            var pathC = result.ShortestRouteNotYetRelaxed;            
            newResult = algorithm.RelaxNode(pathC.EndNode, result);
            Print(newResult);

            var pathD = result.ShortestRouteNotYetRelaxed;            
            newResult = algorithm.RelaxNode(pathD.EndNode, result);
            Print(newResult);

            result.ShortestRouteNotYetRelaxed.Should().BeNull();
        }

        private static void Print(Result result)
        {
            Debug.WriteLine("----------------");
            foreach (var p in result.Routes)
            {
                Debug.WriteLine(p);
            }
        }
    }
}
