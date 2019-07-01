using NUnit.Framework;
using System.Linq;
using TauCode.Algorithms.Graphs;

namespace TauCode.Algorithms.Test
{
    [TestFixture]
    public class NodeTest : GraphTestBase
    {
        [Test]
        public void DrawEdgeTo_ValidNode_DrawsEdge()
        {
            // Arrange
            var coreNode = this.Graph.AddNode("core");
            var mathNode = this.Graph.AddNode("math");
            var webNode = this.Graph.AddNode("web");

            // Act
            var mathCoreEdge = mathNode.DrawEdgeTo(coreNode);

            var webCoreEdge = webNode.DrawEdgeTo(coreNode);
            var webMathEdge = webNode.DrawEdgeTo(mathNode);

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
                new Node<string>[] { },
                new Edge<string>[] { },
                new Node<string>[] { mathNode, webNode },
                new Edge<string>[] { mathCoreEdge, webCoreEdge });

            // "math"
            this.Graph.AssertNode(
                mathNode,
                new Node<string>[] { coreNode },
                new Edge<string>[] { mathCoreEdge },
                new Node<string>[] { webNode },
                new Edge<string>[] { webMathEdge });

            // "web"
            this.Graph.AssertNode(
                webNode,
                new Node<string>[] { coreNode, mathNode },
                new Edge<string>[] { webCoreEdge, webMathEdge },
                new Node<string>[] { },
                new Edge<string>[] { });
        }
    }
}
