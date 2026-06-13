namespace DijkstraAlgorithm.Tests;

using NUnit.Framework;
using Shouldly;

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

        nodeA.ConnectedEdges.Count().ShouldBe(1);
        nodeA.ConnectedEdges.All(c => c.Node == nodeB).ShouldBeTrue();
    }
}
