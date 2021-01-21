using NUnit.Framework;
using DijkstraAlgorithm;
using System;
using FluentAssertions;
using System.Linq;

namespace DijkstraAlgorithm.Tests
{
    public class NodeTests
    {
        [Test]
        public void AddConnectionShouldBeAllowedToAddSelf()
        {
            var nodeA = new Node("A");

            Assert.Throws<ArgumentException>(() =>
            {
                nodeA.AddConnection(nodeA, 10);
            });
        }

        [Test]
        public void AddConnectionShouldBeAllowedToOtherNode()
        {
            var nodeA = new Node("A");
            var nodeB = new Node("B");
            nodeA.AddConnection(nodeB, 10);

            nodeA.Connections.Count().Should().Be(1);
            nodeA.Connections.All(c => c.Node == nodeB).Should().BeTrue();
        }
    }
}