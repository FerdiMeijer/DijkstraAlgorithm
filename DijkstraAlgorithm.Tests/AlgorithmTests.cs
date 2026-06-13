namespace DijkstraAlgorithm.Tests;

using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using Shouldly;

public class AlgorithmTests
{
    [Test]
    public void TestFromAtoBtoCtoD()
    {
        var shape = @"
   A --10--> B --10--> C --10--> D
";
        var nodeA = new Node("A");
        var nodeB = new Node("B");
        var nodeC = new Node("C");
        var nodeD = new Node("D");

        nodeA.AddConnection(nodeB, 10);
        nodeB.AddConnection(nodeC, 10);
        nodeC.AddConnection(nodeD, 10);

        var nodes = new List<Node> { nodeA, nodeB, nodeC, nodeD };
        var algorithm = new ShortestPathAlgorithm(
            nodes,
            NullLogger<ShortestPathAlgorithm>.Instance
        );

        var result = algorithm.Run(nodeA);
        result.ShouldNotBeNull();
        Print(result, shape);
    }

    [Test]
    public void TestWith9Nodes()
    {
        // https://www.geeksforgeeks.org/dijkstras-shortest-path-algorithm-greedy-algo-7/
        var shape = @"
            8           7
      B -------- C -------- D
     /|          |\         |\
  4 / |          | \2       | \ 9
   /  |          |  \       |  \
  A   | 11     4 |   \   14 |   E
   \  |          |    \     |  /
  8 \ |          |     \    | / 10
     \|          |      \   |/
      H -------- I ------ F
       \    7        6    |
        \                 | 2
       1 \                |
          G ---------------
                  6
";
        var nodeA = new Node("A");
        var nodeB = new Node("B");
        var nodeC = new Node("C");
        var nodeD = new Node("D");
        var nodeE = new Node("E");
        var nodeF = new Node("F");
        var nodeG = new Node("G");
        var nodeH = new Node("H");
        var nodeI = new Node("I");

        AddTwoWayConnection(nodeA, nodeB, 4);
        AddTwoWayConnection(nodeA, nodeH, 8);

        AddTwoWayConnection(nodeB, nodeH, 11);
        AddTwoWayConnection(nodeB, nodeC, 8);

        AddTwoWayConnection(nodeC, nodeF, 4);
        AddTwoWayConnection(nodeC, nodeI, 2);
        AddTwoWayConnection(nodeC, nodeD, 7);

        AddTwoWayConnection(nodeD, nodeF, 14);
        AddTwoWayConnection(nodeD, nodeE, 9);

        AddTwoWayConnection(nodeE, nodeF, 10);

        AddTwoWayConnection(nodeF, nodeG, 2);

        AddTwoWayConnection(nodeG, nodeI, 6);
        AddTwoWayConnection(nodeG, nodeH, 1);

        AddTwoWayConnection(nodeH, nodeI, 7);

        var nodes = new List<Node>
        {
            nodeA,
            nodeB,
            nodeC,
            nodeD,
            nodeE,
            nodeF,
            nodeG,
            nodeH,
            nodeI,
        };
        var algorithm = new ShortestPathAlgorithm(
            nodes,
            NullLogger<ShortestPathAlgorithm>.Instance
        );

        var result = algorithm.Run(nodeA);
        result.ShouldNotBeNull();
        Print(result, shape);
    }

    [Test]
    public void TestSingleNode()
    {
        var shape = @"
   (A)
";
        var nodeA = new Node("A");

        var nodes = new List<Node> { nodeA };
        var algorithm = new ShortestPathAlgorithm(
            nodes,
            NullLogger<ShortestPathAlgorithm>.Instance
        );

        var result = algorithm.Run(nodeA);

        result.ShouldNotBeNull();
        result.Iterations.ShouldBe(1);
        result.Paths.Single().Cost.ShouldBe(0);
        Print(result, shape);
    }

    [Test]
    public void TestDisconnectedNodes()
    {
        var shape = @"
   A --5--> B       C (isolated)
";
        var nodeA = new Node("A");
        var nodeB = new Node("B");
        var nodeC = new Node("C");

        nodeA.AddConnection(nodeB, 5);

        var nodes = new List<Node> { nodeA, nodeB, nodeC };
        var algorithm = new ShortestPathAlgorithm(
            nodes,
            NullLogger<ShortestPathAlgorithm>.Instance
        );

        var result = algorithm.Run(nodeA);

        result.ShouldNotBeNull();
        result.Paths.GetPathToNode(nodeA).Cost.ShouldBe(0);
        result.Paths.GetPathToNode(nodeB).Cost.ShouldBe(5);
        result.Paths.GetPathToNode(nodeC).Cost.ShouldBe(double.PositiveInfinity);
        Print(result, shape);
    }

    [Test]
    public void TestDiamondGraph()
    {
        var shape = @"
        B
       /1\
      /   \1
     A     D
      \   /
      5\ /1
        C
";
        var nodeA = new Node("A");
        var nodeB = new Node("B");
        var nodeC = new Node("C");
        var nodeD = new Node("D");

        nodeA.AddConnection(nodeB, 1);
        nodeA.AddConnection(nodeC, 5);
        nodeB.AddConnection(nodeD, 1);
        nodeC.AddConnection(nodeD, 1);

        var nodes = new List<Node> { nodeA, nodeB, nodeC, nodeD };
        var algorithm = new ShortestPathAlgorithm(
            nodes,
            NullLogger<ShortestPathAlgorithm>.Instance
        );

        var result = algorithm.Run(nodeA);

        result.ShouldNotBeNull();
        // Shortest path to D is A->B->D (cost 2), not A->C->D (cost 6)
        result.Paths.GetPathToNode(nodeD).Cost.ShouldBe(2);
        Print(result, shape);
    }

    [Test]
    public void TestMultipleEqualPaths()
    {
        var shape = @"
        B
       /2\
      /   \3
     A     D
      \   /
      2\ /3
        C

  A->B->D = 5, A->C->D = 5 (equal cost)
";
        var nodeA = new Node("A");
        var nodeB = new Node("B");
        var nodeC = new Node("C");
        var nodeD = new Node("D");

        nodeA.AddConnection(nodeB, 2);
        nodeA.AddConnection(nodeC, 2);
        nodeB.AddConnection(nodeD, 3);
        nodeC.AddConnection(nodeD, 3);

        var nodes = new List<Node> { nodeA, nodeB, nodeC, nodeD };
        var algorithm = new ShortestPathAlgorithm(
            nodes,
            NullLogger<ShortestPathAlgorithm>.Instance
        );

        var result = algorithm.Run(nodeA);

        result.ShouldNotBeNull();
        // Both paths have cost 5, algorithm should find one of them
        result.Paths.GetPathToNode(nodeD).Cost.ShouldBe(5);
        Print(result, shape);
    }

    [Test]
    public void TestDirectPathNotShortest()
    {
        var shape = @"
     1       1
  A ----> B ----> C
  |               ^
  +------10-------+
";
        var nodeA = new Node("A");
        var nodeB = new Node("B");
        var nodeC = new Node("C");

        nodeA.AddConnection(nodeC, 10);
        nodeA.AddConnection(nodeB, 1);
        nodeB.AddConnection(nodeC, 1);

        var nodes = new List<Node> { nodeA, nodeB, nodeC };
        var algorithm = new ShortestPathAlgorithm(
            nodes,
            NullLogger<ShortestPathAlgorithm>.Instance
        );

        var result = algorithm.Run(nodeA);

        result.ShouldNotBeNull();
        // Should find A->B->C (cost 2), not A->C (cost 10)
        result.Paths.GetPathToNode(nodeC).Cost.ShouldBe(2);
        Print(result, shape);
    }

    [Test]
    public void TestFullyConnectedGraph()
    {
        var shape = @"
      A
     /|\
    1 2 3
   /  |  \
  B --1-- C
   \  |  /
    2 1 1
     \|/
      D
";
        var nodeA = new Node("A");
        var nodeB = new Node("B");
        var nodeC = new Node("C");
        var nodeD = new Node("D");

        AddTwoWayConnection(nodeA, nodeB, 1);
        AddTwoWayConnection(nodeA, nodeC, 2);
        AddTwoWayConnection(nodeA, nodeD, 3);
        AddTwoWayConnection(nodeB, nodeC, 1);
        AddTwoWayConnection(nodeB, nodeD, 2);
        AddTwoWayConnection(nodeC, nodeD, 1);

        var nodes = new List<Node> { nodeA, nodeB, nodeC, nodeD };
        var algorithm = new ShortestPathAlgorithm(
            nodes,
            NullLogger<ShortestPathAlgorithm>.Instance
        );

        var result = algorithm.Run(nodeA);

        result.ShouldNotBeNull();
        result.Paths.GetPathToNode(nodeA).Cost.ShouldBe(0);
        result.Paths.GetPathToNode(nodeB).Cost.ShouldBe(1);
        result.Paths.GetPathToNode(nodeC).Cost.ShouldBe(2);
        result.Paths.GetPathToNode(nodeD).Cost.ShouldBe(3);
        Print(result, shape);
    }

    [Test]
    public void TestLongChain()
    {
        var shape = @"
  A --1--> B --2--> C --3--> D --4--> E --5--> F
";
        var nodeA = new Node("A");
        var nodeB = new Node("B");
        var nodeC = new Node("C");
        var nodeD = new Node("D");
        var nodeE = new Node("E");
        var nodeF = new Node("F");

        nodeA.AddConnection(nodeB, 1);
        nodeB.AddConnection(nodeC, 2);
        nodeC.AddConnection(nodeD, 3);
        nodeD.AddConnection(nodeE, 4);
        nodeE.AddConnection(nodeF, 5);

        var nodes = new List<Node> { nodeA, nodeB, nodeC, nodeD, nodeE, nodeF };
        var algorithm = new ShortestPathAlgorithm(
            nodes,
            NullLogger<ShortestPathAlgorithm>.Instance
        );

        var result = algorithm.Run(nodeA);

        result.ShouldNotBeNull();
        result.Paths.GetPathToNode(nodeF).Cost.ShouldBe(15); // 1+2+3+4+5
        result.Iterations.ShouldBe(6);
        Print(result, shape);
    }

    [Test]
    public void TestStarTopology()
    {
        var shape = @"
           B
           |1
           |
    E --4-- A --2-- C
           |
           |3
           D
";
        var nodeA = new Node("A");
        var nodeB = new Node("B");
        var nodeC = new Node("C");
        var nodeD = new Node("D");
        var nodeE = new Node("E");

        nodeA.AddConnection(nodeB, 1);
        nodeA.AddConnection(nodeC, 2);
        nodeA.AddConnection(nodeD, 3);
        nodeA.AddConnection(nodeE, 4);

        var nodes = new List<Node> { nodeA, nodeB, nodeC, nodeD, nodeE };
        var algorithm = new ShortestPathAlgorithm(
            nodes,
            NullLogger<ShortestPathAlgorithm>.Instance
        );

        var result = algorithm.Run(nodeA);

        result.ShouldNotBeNull();
        result.Paths.GetPathToNode(nodeB).Cost.ShouldBe(1);
        result.Paths.GetPathToNode(nodeC).Cost.ShouldBe(2);
        result.Paths.GetPathToNode(nodeD).Cost.ShouldBe(3);
        result.Paths.GetPathToNode(nodeE).Cost.ShouldBe(4);
        Print(result, shape);
    }

    [Test]
    public void TestStartFromMiddleNode()
    {
        var shape = @"
  A --1--> B --2--> C --3--> D
           ^
           |
         start

  (A is unreachable from B)
";
        var nodeA = new Node("A");
        var nodeB = new Node("B");
        var nodeC = new Node("C");
        var nodeD = new Node("D");

        nodeA.AddConnection(nodeB, 1);
        nodeB.AddConnection(nodeC, 2);
        nodeC.AddConnection(nodeD, 3);

        var nodes = new List<Node> { nodeA, nodeB, nodeC, nodeD };
        var algorithm = new ShortestPathAlgorithm(
            nodes,
            NullLogger<ShortestPathAlgorithm>.Instance
        );

        var result = algorithm.Run(nodeB);

        result.ShouldNotBeNull();
        result.Paths.GetPathToNode(nodeA).Cost.ShouldBe(double.PositiveInfinity);
        result.Paths.GetPathToNode(nodeB).Cost.ShouldBe(0);
        result.Paths.GetPathToNode(nodeC).Cost.ShouldBe(2);
        result.Paths.GetPathToNode(nodeD).Cost.ShouldBe(5);
        Print(result, shape);
    }

    [Test]
    public void TestWithDecimalWeights()
    {
        var shape = @"
       0.5      1.5
  A -------> B -------> C
  |                     ^
  +--------2.5----------+
";
        var nodeA = new Node("A");
        var nodeB = new Node("B");
        var nodeC = new Node("C");

        nodeA.AddConnection(nodeB, 0.5);
        nodeB.AddConnection(nodeC, 1.5);
        nodeA.AddConnection(nodeC, 2.5);

        var nodes = new List<Node> { nodeA, nodeB, nodeC };
        var algorithm = new ShortestPathAlgorithm(
            nodes,
            NullLogger<ShortestPathAlgorithm>.Instance
        );

        var result = algorithm.Run(nodeA);

        result.ShouldNotBeNull();
        // A->B->C (0.5+1.5=2.0) is shorter than A->C (2.5)
        result.Paths.GetPathToNode(nodeC).Cost.ShouldBe(2.0);
        Print(result, shape);
    }

    private static void AddTwoWayConnection(Node nodeA, Node nodeB, double cost)
    {
        nodeA.AddConnection(nodeB, cost);
        nodeB.AddConnection(nodeA, cost);
    }

    private static void Print(ShortestPaths result, string shape)
    {
        TestContext.Out.WriteLine("================");
        TestContext.Out.WriteLine("Network shape:");
        TestContext.Out.WriteLine(shape);
        TestContext.Out.WriteLine($"Iterations: {result.Iterations}");
        TestContext.Out.WriteLine("Paths found:");
        foreach (var p in result.Paths)
        {
            TestContext.Out.WriteLine($"  {p}");
        }
        TestContext.Out.WriteLine("================");
    }
}
