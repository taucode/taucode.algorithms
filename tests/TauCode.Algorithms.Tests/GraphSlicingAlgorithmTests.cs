﻿using NUnit.Framework;
using System;
using System.Linq;
using TauCode.Algorithms.Graphs;

namespace TauCode.Algorithms.Tests
{
    [TestFixture]
    public class GraphSlicingAlgorithmTests : GraphTestBase
    {
        [Test]
        public void Constructor_ValidArgument_RunsOk()
        {
            // Arrange

            // Act
            var algorithm = new GraphSlicingAlgorithm<string>(this.Graph);

            // Assert
        }

        [Test]
        public void Constructor_ArgumentIsNull_ThrowsArgumentNullException()
        {
            // Arrange

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => new GraphSlicingAlgorithm<string>(null));

            Assert.That(ex.ParamName, Is.EqualTo("graph"));
        }

        [Test]
        public void Slice_CoupledGraph_ReturnsSlices()
        {
            // Arrange
            var a = this.Graph.AddNode("a");
            var b = this.Graph.AddNode("b");
            var c = this.Graph.AddNode("c");
            var d = this.Graph.AddNode("d");
            var e = this.Graph.AddNode("e");
            var f = this.Graph.AddNode("f");
            var g = this.Graph.AddNode("g");
            var h = this.Graph.AddNode("h");
            var i = this.Graph.AddNode("i");
            var j = this.Graph.AddNode("j");
            var k = this.Graph.AddNode("k");
            var l = this.Graph.AddNode("l");
            var m = this.Graph.AddNode("m");
            var n = this.Graph.AddNode("n");
            var o = this.Graph.AddNode("o");
            var p = this.Graph.AddNode("p");
            var q = this.Graph.AddNode("q");

            d.LinkTo(a);
            e.LinkTo(f);
            g.LinkTo(e, p);
            h.LinkTo(d, e);
            i.LinkTo(a);
            j.LinkTo(f);
            k.LinkTo(c);
            l.LinkTo(g);
            m.LinkTo(n);
            n.LinkTo(m);
            o.LinkTo(j);
            p.LinkTo(l);
            q.LinkTo(i);

            // Act
            var slicer = new GraphSlicingAlgorithm<string>(this.Graph);
            var result = slicer.Slice();

            // Assert
            Assert.That(result, Has.Length.EqualTo(4));

            // 0
            CollectionAssert.AreEquivalent(
                result[0].Nodes
                    .Select(x => x.Value)
                    .OrderBy(x => x)
                    .ToArray(),
                new string[] { "a", "b", "c", "f" });
            Assert.That(result[0].Edges, Is.Empty);

            // 1
            CollectionAssert.AreEquivalent(
                result[1].Nodes
                    .Select(x => x.Value)
                    .OrderBy(x => x)
                    .ToArray(),
                new string[] { "d", "e", "i", "j", "k" });
            Assert.That(result[1].Edges, Is.Empty);

            // 2
            CollectionAssert.AreEquivalent(
                result[2].Nodes
                    .Select(x => x.Value)
                    .OrderBy(x => x)
                    .ToArray(),
                new string[] { "h", "o", "q" });
            Assert.That(result[2].Edges, Is.Empty);

            // 3
            CollectionAssert.AreEquivalent(
                result[3].Nodes
                    .Select(x => x.Value)
                    .OrderBy(x => x)
                    .ToArray(),
                new string[] { "m", "n", "g", "l", "p" }.OrderBy(x => x));
            Assert.That(result[3].Edges, Has.Count.EqualTo(5));

            var clonedM = result[3].GetNode("m");
            var clonedN = result[3].GetNode("n");
            var clonedG = result[3].GetNode("g");
            var clonedL = result[3].GetNode("l");
            var clonedP = result[3].GetNode("p");

            var clonedEdgeMN = clonedM.OutgoingEdges.Single();
            var clonedEdgeNM = clonedN.OutgoingEdges.Single();
            var clonedEdgePL = clonedP.OutgoingEdges.Single();
            var clonedEdgeLG = clonedL.OutgoingEdges.Single();
            var clonedEdgeGP = clonedG.OutgoingEdges.Single();


            result[3].AssertNode(
                clonedM,
                new Node<string>[] { clonedN },
                new Edge<string>[] { clonedEdgeMN },
                new Node<string>[] { clonedN },
                new Edge<string>[] { clonedEdgeNM });

            result[3].AssertNode(
                clonedN,
                new Node<string>[] { clonedM },
                new Edge<string>[] { clonedEdgeNM },
                new Node<string>[] { clonedM },
                new Edge<string>[] { clonedEdgeMN });

            result[3].AssertNode(
                clonedP,
                new Node<string>[] { clonedL },
                new Edge<string>[] { clonedEdgePL },
                new Node<string>[] { clonedG },
                new Edge<string>[] { clonedEdgeGP });

            result[3].AssertNode(
                clonedL,
                new Node<string>[] { clonedG },
                new Edge<string>[] { clonedEdgeLG },
                new Node<string>[] { clonedP },
                new Edge<string>[] { clonedEdgePL });

            result[3].AssertNode(
                clonedG,
                new Node<string>[] { clonedP },
                new Edge<string>[] { clonedEdgeGP },
                new Node<string>[] { clonedL },
                new Edge<string>[] { clonedEdgeLG });
        }

        [Test]
        public void Slice_EmptyGraph_ReturnsEmptyResult()
        {
            // Arrange

            // Act
            var slicer = new GraphSlicingAlgorithm<string>(this.Graph);
            var result = slicer.Slice();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Slice_DecoupledGraph_ReturnsResultWithSameGraph()
        {
            // Arrange
            var a = this.Graph.AddNode("a");
            var b = this.Graph.AddNode("b");
            var c = this.Graph.AddNode("c");
            var d = this.Graph.AddNode("d");
            var e = this.Graph.AddNode("e");

            // Act
            var slicer = new GraphSlicingAlgorithm<string>(this.Graph);
            var result = slicer.Slice();

            // Assert
            Assert.That(result, Has.Length.EqualTo(1));

            // 0
            CollectionAssert.AreEquivalent(
                result[0].Nodes
                    .Select(x => x.Value)
                    .OrderBy(x => x)
                    .ToArray(),
                new string[] { "a", "b", "c", "d", "e" });
            Assert.That(result[0].Edges, Is.Empty);
        }

        [Test]
        public void Slice_LightlyCoupledGraph_ReturnsResultWithSameGraph()
        {
            // Arrange
            var a = this.Graph.AddNode("a");
            var b = this.Graph.AddNode("b");
            var c = this.Graph.AddNode("c");
            var d = this.Graph.AddNode("d");
            var e = this.Graph.AddNode("e");

            var w = this.Graph.AddNode("w");
            var y = this.Graph.AddNode("y");
            var z = this.Graph.AddNode("z");

            w.LinkTo(a, b);
            y.LinkTo(d, e, a);
            z.LinkTo(e, c);

            // Act
            var slicer = new GraphSlicingAlgorithm<string>(this.Graph);
            var result = slicer.Slice();

            // Assert
            Assert.That(result, Has.Length.EqualTo(2));

            // 0
            CollectionAssert.AreEquivalent(
                result[0].Nodes
                    .Select(x => x.Value)
                    .OrderBy(x => x)
                    .ToArray(),
                new string[] { "a", "b", "c", "d", "e" }.OrderBy(x => x));
            Assert.That(result[0].Edges, Is.Empty);

            // 0
            CollectionAssert.AreEquivalent(
                result[1].Nodes
                    .Select(x => x.Value)
                    .OrderBy(x => x)
                    .ToArray(),
                new string[] { "w", "y", "z" }.OrderBy(x => x));
            Assert.That(result[1].Edges, Is.Empty);

        }
    }
}