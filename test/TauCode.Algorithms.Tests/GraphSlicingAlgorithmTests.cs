using NUnit.Framework;
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
                new string[] { "a", "b", "c", "f" },
                result[0].Nodes
                    .Select(x => x.Value)
                    .OrderBy(x => x)
                    .ToArray()
                );
            Assert.That(result[0].GetEdges(), Is.Empty);

            // 1
            CollectionAssert.AreEquivalent(
                new string[] { "d", "e", "i", "j", "k" },
                result[1].Nodes
                    .Select(x => x.Value)
                    .OrderBy(x => x)
                    .ToArray()
                );
            Assert.That(result[1].GetEdges(), Is.Empty);

            // 2
            CollectionAssert.AreEquivalent(
                new string[] { "h", "o", "q" },
                result[2].Nodes
                    .Select(x => x.Value)
                    .OrderBy(x => x)
                    .ToArray()
                );
            Assert.That(result[2].GetEdges(), Is.Empty);

            // 3
            CollectionAssert.AreEquivalent(
                new string[] { "m", "n", "g", "l", "p" }/*.OrderBy(x => x)*/,
                result[3].Nodes
                    .Select(x => x.Value)
                    .OrderBy(x => x)
                    .ToArray()
                );

            Assert.That(result[3].GetEdges(), Has.Count.EqualTo(5));

            var clonedM = result[3].GetNode("m");
            var clonedN = result[3].GetNode("n");
            var clonedG = result[3].GetNode("g");
            var clonedL = result[3].GetNode("l");
            var clonedP = result[3].GetNode("p");

            var clonedEdgeMN = clonedM.GetOutgoingEdgesLyingInGraph(result[3]).Single();
            var clonedEdgeNM = clonedN.GetOutgoingEdgesLyingInGraph(result[3]).Single();
            var clonedEdgePL = clonedP.GetOutgoingEdgesLyingInGraph(result[3]).Single();
            var clonedEdgeLG = clonedL.GetOutgoingEdgesLyingInGraph(result[3]).Single();
            var clonedEdgeGP = clonedG.GetOutgoingEdgesLyingInGraph(result[3]).Single();

            result[3].AssertNode(
                clonedM,
                new INode<string>[] { clonedN },
                new IEdge<string>[] { clonedEdgeMN },
                new INode<string>[] { clonedN },
                new IEdge<string>[] { clonedEdgeNM });

            result[3].AssertNode(
                clonedN,
                new INode<string>[] { clonedM },
                new IEdge<string>[] { clonedEdgeNM },
                new INode<string>[] { clonedM },
                new IEdge<string>[] { clonedEdgeMN });

            result[3].AssertNode(
                clonedP,
                new INode<string>[] { clonedL },
                new IEdge<string>[] { clonedEdgePL },
                new INode<string>[] { clonedG },
                new IEdge<string>[] { clonedEdgeGP });

            result[3].AssertNode(
                clonedL,
                new INode<string>[] { clonedG },
                new IEdge<string>[] { clonedEdgeLG },
                new INode<string>[] { clonedP },
                new IEdge<string>[] { clonedEdgePL });

            result[3].AssertNode(
                clonedG,
                new INode<string>[] { clonedP },
                new IEdge<string>[] { clonedEdgeGP },
                new INode<string>[] { clonedL },
                new IEdge<string>[] { clonedEdgeLG });
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
                new string[] { "a", "b", "c", "d", "e" },
                result[0].Nodes
                    .Select(x => x.Value)
                    .OrderBy(x => x)
                    .ToArray()
                );
            Assert.That(result[0].GetEdges(), Is.Empty);
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
                new string[] { "a", "b", "c", "d", "e" }.OrderBy(x => x),
                result[0].Nodes
                    .Select(x => x.Value)
                    .OrderBy(x => x)
                    .ToArray()
                );
            Assert.That(result[0].GetEdges(), Is.Empty);

            // 0
            CollectionAssert.AreEquivalent(
                new string[] { "w", "y", "z" }.OrderBy(x => x),
                result[1].Nodes
                    .Select(x => x.Value)
                    .OrderBy(x => x)
                    .ToArray()
                );
            Assert.That(result[1].GetEdges(), Is.Empty);

        }

        [Test]
        public void Slice_SelfReference_ReturnsValidSlices()
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

            a.LinkTo(a);
            e.LinkTo(e);

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
                new string[] { "a", "b", "c", "d", "e" }.OrderBy(x => x),
                result[0].Nodes
                    .Select(x => x.Value)
                    .OrderBy(x => x)
                    .ToArray()
                );

            var edges = result[0].GetEdges();
            Assert.That(edges, Has.Count.EqualTo(2));

            var edge = edges.Single(x => x.From == a);
            Assert.That(edge.To, Is.EqualTo(a));

            edge = edges.Single(x => x.From == e);
            Assert.That(edge.To, Is.EqualTo(e));

            // 0
            CollectionAssert.AreEquivalent(
                new string[] { "w", "y", "z" }.OrderBy(x => x),
                result[1].Nodes
                    .Select(x => x.Value)
                    .OrderBy(x => x)
                    .ToArray()
                );
            Assert.That(result[1].GetEdges(), Is.Empty);
        }
    }
}
