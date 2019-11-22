using System;
using System.Linq;
using NUnit.Framework;
using TauCode.Algorithms.Graphs;

namespace TauCode.Algorithms.Tests
{
    internal static class TestHelper
    {
        internal static Node<string> GetNode(this Graph<string> graph, string nodeValue)
        {
            return graph.Nodes.Single(x => x.Value == nodeValue);
        }

        internal static void AssertNode(
            this Graph<string> graph,
            Node<string> node,
            Node<string>[] linkedToNodes,
            Edge<string>[] linkedToEdges,
            Node<string>[] linkedFromNodes,
            Edge<string>[] linkedFromEdges)
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

        internal static Edge<string>[] LinkFrom(this Graph<string> graph, Node<string> node, Node<string>[] fromNodes)
        {
            return fromNodes
                .Select(fromNode => fromNode.DrawEdgeTo(node))
                .ToArray();
        }

        internal static Edge<string>[] LinkTo(this Node<string> node, params Node<string>[] otherNodes)
        {
            return otherNodes
                .Select(node.DrawEdgeTo)
                .ToArray();
        }

        internal static Node<string> AssertNodeExists(this Graph<string> graph, string nodeValue)
        {
            var node = graph.Nodes.Single(x => x.Value == nodeValue);
            return node;
        }

        internal static void AssertEdgesExist(this Graph<string> graph, Node<string> node, Node<string>[] linkFromNodes)
        {
            Assert.That(node.Graph, Is.SameAs(graph));

            foreach (var fromNode in linkFromNodes)
            {
                Assert.That(fromNode.Graph, Is.SameAs(graph));

                var edge = node.IncomingEdges.Single(x => x.From == fromNode);
                Assert.That(edge.To, Is.SameAs(node));
                Assert.That(fromNode.OutgoingEdges, Does.Contain(edge));

                Assert.That(graph.Edges, Does.Contain(edge));
            }
        }

        internal static bool EdgeIsDetached<T>(this Edge<T> edge)
        {
            return
                edge.From == null &&
                edge.To == null;
        }
    }
}
