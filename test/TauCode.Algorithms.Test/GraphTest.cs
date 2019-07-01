using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Algorithms.Graphs;

namespace TauCode.Algorithms.Test
{
    [TestFixture]
    public class GraphTest : GraphTestBase
    {
        #region Constructor

        [Test]
        public void Constructor_NoArguments_CreatesGraph()
        {
            // Arrange

            // Act
            var graph = new Graph<string>();

            // Assert
            Assert.That(graph.Nodes, Is.Not.Null);
            Assert.That(graph.Nodes, Is.Empty);

            Assert.That(graph.Edges, Is.Not.Null);
            Assert.That(graph.Edges, Is.Empty);
        }

        #endregion

        #region AddNode

        [Test]
        public void Add_NodeValue_AddsNodeWithProvidedValue()
        {
            // Arrange
            var graph = new Graph<string>();

            // Act
            var coreNode = graph.AddNode("core");
            var mathNode = graph.AddNode("math");

            // Assert
            Assert.That(graph.Nodes, Has.Count.EqualTo(2));

            var assertCoreNode = graph.Nodes.SingleOrDefault(x => x.Value == "core");
            Assert.That(assertCoreNode, Is.Not.Null);
            Assert.That(assertCoreNode, Is.SameAs(coreNode));
            Assert.That(assertCoreNode.Value, Is.EqualTo("core"));
            Assert.That(assertCoreNode.Graph, Is.SameAs(graph));

            var assertMathNode = graph.Nodes.SingleOrDefault(x => x.Value == "math");
            Assert.That(assertMathNode, Is.Not.Null);
            Assert.That(assertMathNode, Is.SameAs(mathNode));
            Assert.That(assertMathNode.Value, Is.EqualTo("math"));
            Assert.That(assertMathNode.Graph, Is.SameAs(graph));
        }

        #endregion

        #region RemoveNode

        [Test]
        public void RemoveNode_ValidNode_RemovesNode()
        {
            // Arrange
            var coreNode = this.Graph.AddNode("core");
            var mathNode = this.Graph.AddNode("math");
            var webNode = this.Graph.AddNode("web");

            var mathCoreEdge = mathNode.DrawEdgeTo(coreNode);

            var webCoreEdge = webNode.DrawEdgeTo(coreNode);
            var webMathEdge = webNode.DrawEdgeTo(mathNode);

            // Act
            this.Graph.RemoveNode(mathNode);

            // Assert
            Assert.That(this.Graph.Nodes, Has.Count.EqualTo(2));
            Assert.That(this.Graph.Edges, Has.Count.EqualTo(1));

            // "core"
            this.Graph.AssertNode(
                coreNode,
                new Node<string>[] { },
                new Edge<string>[] { },
                new Node<string>[] { webNode },
                new Edge<string>[] { webCoreEdge });

            // "web"
            this.Graph.AssertNode(
                webNode,
                new Node<string>[] { coreNode, },
                new Edge<string>[] { webCoreEdge, },
                new Node<string>[] { },
                new Edge<string>[] { });

            // deleted "math"
            var deletedMath = this.Graph.Nodes.SingleOrDefault(x => x.Value == "math");
            Assert.That(deletedMath, Is.Null);

            Assert.That(mathNode.Graph, Is.Null);
            IReadOnlyCollection<Edge<string>> dummy;
            var ex = Assert.Throws<InvalidOperationException>(() => dummy = mathNode.OutgoingEdges);
            Assert.That(ex.Message, Is.EqualTo("Node is detached"));
        }

        [Test]
        public void RemoveNode_ArgumentIsNull_ThrowsArgumentNullException()
        {
            // Arrange

            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => this.Graph.RemoveNode(null));

            // Assert
            Assert.That(ex.ParamName, Is.EqualTo("node"));
        }

        [Test]
        public void RemoveNode_ArgumentIsFromOtherGraph_ThrowsInvalidOperationException()
        {
            // Arrange
            var otherGraph = new Graph<string>();
            var otherNode = otherGraph.AddNode("foo");

            // Act
            var ex = Assert.Throws<InvalidOperationException>(() => this.Graph.RemoveNode(otherNode));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Node does not belong to the given graph"));
        }

        [Test]
        public void RemoveNode_ArgumentIsDetached_ThrowsInvalidOperationException()
        {
            // Arrange
            var detachedNode = this.Graph.AddNode("detached");
            this.Graph.RemoveNode(detachedNode);

            // Act
            var ex = Assert.Throws<InvalidOperationException>(() => this.Graph.RemoveNode(detachedNode));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Node does not belong to the given graph"));
        }

        #endregion

        #region Clone

        [Test]
        public void Clone_NoArguments_ClonesGraph()
        {
            // Arrange
            var coreNode = this.Graph.AddNode("core");
            var mathNode = this.Graph.AddNode("math");
            var webNode = this.Graph.AddNode("web");

            var coreEdges = this.Graph.LinkFrom(coreNode, new[] { mathNode, webNode });
            var mathEdges = this.Graph.LinkFrom(mathNode, new[] { webNode });

            // Act
            var clonedGraph = this.Graph.Clone();

            // Assert
            Assert.That(clonedGraph.Nodes, Has.Count.EqualTo(3));
            Assert.That(clonedGraph.Edges, Has.Count.EqualTo(3));

            // "core"
            var clonedCoreNode = clonedGraph.AssertNodeExists("core");
            var clonedMathNode = clonedGraph.AssertNodeExists("math");
            var clonedWebNode = clonedGraph.AssertNodeExists("web");

            clonedGraph.AssertEdgesExist(clonedCoreNode, new Node<string>[] { clonedMathNode, clonedWebNode });
            clonedGraph.AssertEdgesExist(clonedMathNode, new Node<string>[] { clonedWebNode });
        }

        #endregion

        #region RemoveEdge

        [Test]
        public void RemoveEdge_ValidEdge_RemovesEdge()
        {
            // Arrange
            var a = this.Graph.AddNode("a");
            var b = this.Graph.AddNode("b");
            var c = this.Graph.AddNode("c");
            var d = this.Graph.AddNode("d");

            a.LinkTo(b, c);
            var cEdge = c.LinkTo(b).Single();
            d.LinkTo(a);

            // Act
            this.Graph.RemoveEdge(cEdge);

            // Assert
            Assert.That(this.Graph.Edges, Has.Count.EqualTo(3));

            Assert.That(this.Graph.Edges, Does.Not.Contain(cEdge));
            Assert.That(b.IncomingEdges, Does.Not.Contain(cEdge));
            Assert.That(c.OutgoingEdges, Does.Not.Contain(cEdge));

            Assert.That(cEdge.From, Is.Null);
            Assert.That(cEdge.To, Is.Null);
        }

        [Test]
        public void RemoveEdge_SameEdgeTwice_ThrowsWatException()
        {
            // Arrange
            var a = this.Graph.AddNode("a");
            var b = this.Graph.AddNode("b");
            var c = this.Graph.AddNode("c");
            var d = this.Graph.AddNode("d");

            a.LinkTo(b, c);
            var cEdge = c.LinkTo(b).Single();
            d.LinkTo(a);

            this.Graph.RemoveEdge(cEdge);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => this.Graph.RemoveEdge(cEdge));

            Assert.That(ex.Message, Does.StartWith("Edge does not belong to the given graph"));
            Assert.That(ex.ParamName, Is.EqualTo("edge"));
        }

        [Test]
        public void RemoveEdge_EdgeIsNull_ThrowsArgumentNullException()
        {
            // Arrange

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => this.Graph.RemoveEdge(null));

            Assert.That(ex.ParamName, Is.EqualTo("edge"));
        }

        [Test]
        public void RemoveEdge_EdgeFromOtherGraph_ThrowsWatException()
        {
            // Arrange
            var otherGraph = new Graph<string>();
            var a = otherGraph.AddNode("a");
            var b = otherGraph.AddNode("b");
            var otherEdge = a.LinkTo(b).Single();

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => this.Graph.RemoveEdge(otherEdge));

            Assert.That(ex.Message, Does.StartWith("Edge does not belong to the given graph"));
            Assert.That(ex.ParamName, Is.EqualTo("edge"));
        }

        #endregion

        #region CaptureNodes

        [Test]
        public void CaptureNodes_ValidArgument_CapturesNodes()
        {
            // Arrange
            var a = this.Graph.AddNode("a");
            var b = this.Graph.AddNode("b");
            var c = this.Graph.AddNode("c");
            var d = this.Graph.AddNode("d");
            var e = this.Graph.AddNode("e");
            var f = this.Graph.AddNode("f");

            var aEdges = a.LinkTo(b);
            var bEdges = b.LinkTo(c, d);
            var cEdges = c.LinkTo(a, d, f);
            var dEdges = d.LinkTo(e, f, a);
            var eEdges = e.LinkTo(f);

            var otherGraph = new Graph<string>()
            {
                Name = "Predator",
            };

            // Act
            otherGraph.CaptureNodes(new[] { a, b, c });

            // Assert
            // 'Predator' graph
            Assert.That(otherGraph.Nodes, Has.Count.EqualTo(3));
            Assert.That(otherGraph.Edges, Has.Count.EqualTo(3));

            // a
            Assert.That(otherGraph.Nodes, Does.Contain(a));
            Assert.That(otherGraph.Edges, Does.Contain(aEdges[0]));
            otherGraph.AssertNode(
                a,
                new Node<string>[] { b },
                new Edge<string>[] { aEdges[0] },
                new Node<string>[] { c },
                new Edge<string>[] { cEdges[0] });

            // b
            Assert.That(otherGraph.Nodes, Does.Contain(b));
            Assert.That(otherGraph.Edges, Does.Contain(bEdges[0]));
            Assert.That(otherGraph.Edges, Does.Not.Contain(bEdges[1]));
            Assert.That(bEdges[1].EdgeIsDetached(), Is.True);
            otherGraph.AssertNode(
                b,
                new Node<string>[] { c },
                new Edge<string>[] { bEdges[0] },
                new Node<string>[] { a },
                new Edge<string>[] { aEdges[0] });

            // c
            Assert.That(otherGraph.Nodes, Does.Contain(c));
            Assert.That(otherGraph.Edges, Does.Contain(cEdges[0]));
            Assert.That(otherGraph.Edges, Does.Not.Contain(cEdges[1]));
            Assert.That(bEdges[1].EdgeIsDetached(), Is.True);
            Assert.That(otherGraph.Edges, Does.Not.Contain(cEdges[2]));
            Assert.That(cEdges[2].EdgeIsDetached(), Is.True);
            otherGraph.AssertNode(
                c,
                new Node<string>[] { a },
                new Edge<string>[] { cEdges[0] },
                new Node<string>[] { b },
                new Edge<string>[] { bEdges[0] });

            // 'Rest' of graph
            Assert.That(this.Graph.Nodes, Has.Count.EqualTo(3));
            Assert.That(this.Graph.Edges, Has.Count.EqualTo(3));

            // d
            Assert.That(this.Graph.Nodes, Does.Contain(d));
            Assert.That(this.Graph.Edges, Does.Contain(dEdges[0]));
            Assert.That(this.Graph.Edges, Does.Contain(dEdges[1]));
            Assert.That(this.Graph.Edges, Does.Not.Contain(dEdges[2]));
            Assert.That(dEdges[2].EdgeIsDetached(), Is.True);
            this.Graph.AssertNode(
                d,
                new Node<string>[] { e, f },
                new Edge<string>[] { dEdges[0], dEdges[1] },
                new Node<string>[] { },
                new Edge<string>[] { });

            // e
            Assert.That(this.Graph.Nodes, Does.Contain(e));
            Assert.That(this.Graph.Edges, Does.Contain(eEdges[0]));
            this.Graph.AssertNode(
                e,
                new Node<string>[] { f },
                new Edge<string>[] { eEdges[0] },
                new Node<string>[] { d },
                new Edge<string>[] { dEdges[0] });

            // f
            Assert.That(this.Graph.Nodes, Does.Contain(f));
            this.Graph.AssertNode(
                f,
                new Node<string>[] { },
                new Edge<string>[] { },
                new Node<string>[] { e, d },
                new Edge<string>[] { eEdges[0], dEdges[1] });
        }

        [Test]
        public void CaptureNodes_ArgumentIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var otherGraph = new Graph<string>()
            {
                Name = "Predator",
            };

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => otherGraph.CaptureNodes(null));
            Assert.That(ex.ParamName, Is.EqualTo("otherGraphNodes"));
        }

        [Test]
        public void CaptureNodes_ArgumentContainsNulls_ThrowsArgumentException()
        {
            // Arrange
            var a = this.Graph.AddNode("a");
            var b = this.Graph.AddNode("b");
            var c = this.Graph.AddNode("c");
            var d = this.Graph.AddNode("d");
            var e = this.Graph.AddNode("e");
            var f = this.Graph.AddNode("f");

            var aEdges = a.LinkTo(b);
            var bEdges = b.LinkTo(c, d);
            var cEdges = c.LinkTo(a, d, f);
            var dEdges = d.LinkTo(e, f, a);
            var eEdges = e.LinkTo(f);

            var otherGraph = new Graph<string>()
            {
                Name = "Predator",
            };

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => otherGraph.CaptureNodes(new Node<string>[] { a, b, c, null, d }));
            Assert.That(ex.ParamName, Is.EqualTo("otherGraphNodes"));
            Assert.That(ex.Message, Does.StartWith("Nodes cannot contain nulls"));
        }

        [Test]
        public void CaptureNodes_NodesFromDifferentGraphs_ThrowsArgumentException()
        {
            // Arrange
            var oneMoreGraph = new Graph<string>();
            var x = oneMoreGraph.AddNode("x");

            var a = this.Graph.AddNode("a");

            var predatorGraph = new Graph<string>()
            {
                Name = "Predator",
            };

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => predatorGraph.CaptureNodes(new Node<string>[] { a, x }));
            Assert.That(ex.ParamName, Is.EqualTo("otherGraphNodes"));
            Assert.That(ex.Message, Does.StartWith("Nodes must belong to a single graph"));
        }

        [Test]
        public void CaptureNodes_NodesAreDetached_ThrowsArgumentException()
        {
            // Arrange
            var a = this.Graph.AddNode("a");
            var b = this.Graph.AddNode("b");

            this.Graph.RemoveNode(a);
            this.Graph.RemoveNode(b);

            var predatorGraph = new Graph<string>()
            {
                Name = "Predator",
            };

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => predatorGraph.CaptureNodes(new Node<string>[] { a, b }));
            Assert.That(ex.ParamName, Is.EqualTo("otherGraphNodes"));
            Assert.That(ex.Message, Does.StartWith("Nodes cannot contain detached ones"));
        }

        [Test]
        public void CaptureNodes_NodesFromSameGraph_ThrowsArgumentException()
        {
            // Arrange
            var predatorGraph = new Graph<string>()
            {
                Name = "Predator",
            };

            var a = predatorGraph.AddNode("a");
            var b = predatorGraph.AddNode("b");

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => predatorGraph.CaptureNodes(new Node<string>[] { a, b }));
            Assert.That(ex.ParamName, Is.EqualTo("otherGraphNodes"));
            Assert.That(ex.Message, Does.StartWith("Cannot capture own nodes"));
        }

        #endregion
    }
}

