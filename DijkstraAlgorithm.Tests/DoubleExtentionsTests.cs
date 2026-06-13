namespace DijkstraAlgorithm.Tests;

using Shouldly;
using NUnit.Framework;

public class DoubleExtensionsTests
{
    [Test]
    public void AddShouldNotExceedMax()
    {
        double add = 10;
        var value = double.MaxValue - add;

        value.Add(add + 1).ShouldBe(double.MaxValue);
    }

    [Test]
    public void AddShouldAddIfMaxIsNotReached()
    {
        double add = 10;
        double value = 10;
        value.Add(add).ShouldBe(20);
    }
}
