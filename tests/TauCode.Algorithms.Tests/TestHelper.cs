using System;
using System.Linq;
using NUnit.Framework;
using TauCode.Algorithms.Graphs;

namespace TauCode.Algorithms.Tests
{
    internal static class TestHelper
    {
        internal static INode<string> GetNode(this Graph<string> graph, string nodeValue)
        {
            return graph.Nodes.Single(x => x.Value == nodeValue);
        }

        internal static void AssertNode(
            this IGraph<string> graph,
            INode<string> node,
            INode<string>[] linkedToNodes,
            IEdge<string>[] linkedToEdges,
            INode<string>[] linkedFromNodes,
            IEdge<string>[] linkedFromEdges)
        {
            if (linkedToNodes.Length != linkedToEdges.Length)
            {
                throw new ArgumentException();
            }

            if (linkedFromNodes.Length != linkedFromEdges.Length)
            {
                throw new ArgumentException();
            }

            Assert.That(node.Graph, Is.SameAs(graph));

            // check 'outgoing' edges
            Assert.That(node.OutgoingEdges.Count, Is.EqualTo(linkedToNodes.Length));

            foreach (var outgoingEdge in node.OutgoingEdges)
            {
                Assert.That(outgoingEdge.From, Is.EqualTo(node));
                
                var to = outgoingEdge.To;
                Assert.That(to.Graph, Is.EqualTo(graph));
                Assert.That(to.IncomingEdges, Does.Contain(outgoingEdge));

                var index = Array.IndexOf(linkedToNodes, to);
                Assert.That(index, Is.GreaterThanOrEqualTo(0));
                Assert.That(outgoingEdge, Is.SameAs(linkedToEdges[index]));
            }

            // check 'incoming' edges
            Assert.That(node.IncomingEdges.Count, Is.EqualTo(linkedFromNodes.Length));

            foreach (var incomingEdge in node.IncomingEdges)
            {
                Assert.That(incomingEdge.To, Is.EqualTo(node));

                var from = incomingEdge.From;
                Assert.That(from.Graph, Is.EqualTo(graph));
                Assert.That(from.OutgoingEdges, Does.Contain(incomingEdge));

                var index = Array.IndexOf(linkedFromNodes, from);
                Assert.That(index, Is.GreaterThanOrEqualTo(0));
                Assert.That(incomingEdge, Is.SameAs(linkedFromEdges[index]));
            }
        }

        internal static IEdge<string>[] LinkFrom(this IGraph<string> graph, INode<string> node, INode<string>[] fromNodes)
        {
            throw new NotImplementedException();
            //return fromNodes
            //    .Select(fromNode => fromNode.DrawEdgeTo(node))
            //    .ToArray();
        }

        internal static IEdge<string>[] LinkTo(this INode<string> node, params INode<string>[] otherNodes)
        {
            throw new NotImplementedException();
            //return otherNodes
            //    .Select(node.DrawEdgeTo)
            //    .ToArray();
        }

        internal static INode<string> AssertNodeExists(this IGraph<string> graph, string nodeValue)
        {
            throw new NotImplementedException();
            //var node = graph.Nodes.Single(x => x.Value == nodeValue);
            //return node;
        }

        internal static void AssertEdgesExist(this Graph<string> graph, INode<string> node, INode<string>[] linkFromNodes)
        {
            throw new NotImplementedException();

            //Assert.That(node.Graph, Is.SameAs(graph));

            //foreach (var fromNode in linkFromNodes)
            //{
            //    Assert.That(fromNode.Graph, Is.SameAs(graph));

            //    var edge = node.IncomingEdges.Single(x => x.From == fromNode);
            //    Assert.That(edge.To, Is.SameAs(node));
            //    Assert.That(fromNode.OutgoingEdges, Does.Contain(edge));

            //    Assert.That(graph.Edges, Does.Contain(edge));
            //}
        }

        internal static bool EdgeIsDetached<T>(this IEdge<T> edge)
        {
            return
                edge.From == null &&
                edge.To == null;
        }
    }
}
