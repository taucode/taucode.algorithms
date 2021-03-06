﻿using NUnit.Framework;
using System;
using System.Linq;
using TauCode.Algorithms.Graphs;

namespace TauCode.Algorithms.Tests
{
    internal static class TestHelper
    {
        internal static IEdge<string>[] LinkTo(this INode<string> node, params INode<string>[] otherNodes)
        {
            return otherNodes
                .Select(node.DrawEdgeTo)
                .ToArray();
        }

        internal static INode<string> GetNode(this IGraph<string> graph, string nodeValue)
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

            Assert.That(graph.ContainsNode(node), Is.True);

            // check 'outgoing' edges
            Assert.That(node.GetOutgoingEdgesLyingInGraph(graph).Count, Is.EqualTo(linkedToNodes.Length));

            foreach (var outgoingEdge in node.GetOutgoingEdgesLyingInGraph(graph))
            {
                Assert.That(outgoingEdge.From, Is.EqualTo(node));

                var to = outgoingEdge.To;
                Assert.That(graph.ContainsNode(to), Is.True);
                Assert.That(to.IncomingEdges, Does.Contain(outgoingEdge));

                var index = Array.IndexOf(linkedToNodes, to);
                Assert.That(index, Is.GreaterThanOrEqualTo(0));
                Assert.That(outgoingEdge, Is.SameAs(linkedToEdges[index]));
            }

            // check 'incoming' edges
            Assert.That(node.GetIncomingEdgesLyingInGraph(graph).Count, Is.EqualTo(linkedFromNodes.Length));

            foreach (var incomingEdge in node.GetIncomingEdgesLyingInGraph(graph))
            {
                Assert.That(incomingEdge.To, Is.EqualTo(node));

                var from = incomingEdge.From;
                Assert.That(graph.ContainsNode(from), Is.True);
                Assert.That(from.OutgoingEdges, Does.Contain(incomingEdge));

                var index = Array.IndexOf(linkedFromNodes, from);
                Assert.That(index, Is.GreaterThanOrEqualTo(0));
                Assert.That(incomingEdge, Is.SameAs(linkedFromEdges[index]));
            }
        }
    }
}
