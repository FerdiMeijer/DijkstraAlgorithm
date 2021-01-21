using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace DijkstraAlgorithm.Tests
{
    public class DoubleExtentionsTests
    {
        [Test]
        public void AddShouldNotExceedMax()
        {
            double add = 10;
            var value = double.MaxValue - add;

            value.Add(add + 1).Should().Be(double.MaxValue);
        }

        [Test]
        public void AddShouldAddIfMaxIsNotReached()
        {
            double add = 10;
            double value = 10;
            value.Add(add).Should().Be(20);
        }

    }
}
