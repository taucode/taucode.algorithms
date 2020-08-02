using System;
using NUnit.Framework;
using System.Linq;
using TauCode.Algorithms.Graphs;

// todo clean up
namespace TauCode.Algorithms.Tests
{
    [TestFixture]
    public class NodeTests : GraphTestBase
    {
        [Test]
        public void DrawEdgeTo_ValidNode_DrawsEdge()
        {
            // Arrange
            var coreNode = this.Graph.AddNode("core");
            var mathNode = this.Graph.AddNode("math");
            var webNode = this.Graph.AddNode("web");

            // Act
            var mathCoreEdge = this.Graph.DrawEdge(mathNode, coreNode);
            //var mathCoreEdge = mathNode.DrawEdgeTo(coreNode);

            var webCoreEdge = this.Graph.DrawEdge(webNode, coreNode);
            //var webCoreEdge = webNode.DrawEdgeTo(coreNode);

            var webMathEdge = this.Graph.DrawEdge(webNode, mathNode);
            //var webMathEdge = webNode.DrawEdgeTo(mathNode);

            // Assert
            Assert.That(this.Graph.Edges, Has.Count.EqualTo(3));

            var assertMathCoreEdge = this.Graph.Edges.SingleOrDefault(x => x.From.Equals(mathNode) && x.To.Equals(coreNode));
            var assertWebCoreEdge = this.Graph.Edges.SingleOrDefault(x => x.From.Equals(webNode) && x.To.Equals(coreNode));
            var assertWebMathEdge = this.Graph.Edges.SingleOrDefault(x => x.From.Equals(webNode) && x.To.Equals(mathNode));

            Assert.That(assertMathCoreEdge, Is.SameAs(mathCoreEdge));
            Assert.That(assertWebCoreEdge, Is.SameAs(webCoreEdge));
            Assert.That(assertWebMathEdge, Is.SameAs(webMathEdge));

            Assert.That(assertMathCoreEdge, Is.Not.SameAs(assertWebCoreEdge));
            Assert.That(assertMathCoreEdge, Is.Not.SameAs(assertWebMathEdge));
            Assert.That(assertWebCoreEdge, Is.Not.SameAs(assertWebMathEdge));

            // "core"
            this.Graph.AssertNode(
                coreNode,
                new INode<string>[] { },
                new IEdge<string>[] { },
                new INode<string>[] { mathNode, webNode },
                new IEdge<string>[] { mathCoreEdge, webCoreEdge });

            // "math"
            this.Graph.AssertNode(
                mathNode,
                new INode<string>[] { coreNode },
                new IEdge<string>[] { mathCoreEdge },
                new INode<string>[] { webNode },
                new IEdge<string>[] { webMathEdge });

            // "web"
            this.Graph.AssertNode(
                webNode,
                new INode<string>[] { coreNode, mathNode },
                new IEdge<string>[] { webCoreEdge, webMathEdge },
                new INode<string>[] { },
                new IEdge<string>[] { });
        }
    }
}
