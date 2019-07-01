﻿using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TauCode.Algorithms.Graphs
{
    [DebuggerDisplay("{" + nameof(Name) + "}")]
    public class Graph<T>
    {
        #region Nested

        private class NodeData
        {
            internal NodeData(Node<T> node)
            {
                this.Node = node;
                this.OutgoingEdges = new HashSet<Edge<T>>();
                this.IncomingEdges = new HashSet<Edge<T>>();
            }

            private Node<T> Node { get; }
            internal HashSet<Edge<T>> OutgoingEdges { get; }
            internal HashSet<Edge<T>> IncomingEdges { get; }

            internal void AddOutgoingEdge(Edge<T> edge)
            {
                this.OutgoingEdges.Add(edge);
            }

            internal void AddIncomingEdge(Edge<T> edge)
            {
                this.IncomingEdges.Add(edge);
            }
        }

        #endregion

        #region Fields

        private readonly Dictionary<Node<T>, NodeData> _nodes;
        private readonly HashSet<Edge<T>> _edges;

        #endregion

        #region Constructor

        public Graph()
        {
            this.Cloner = DefaultCloner;
            _nodes = new Dictionary<Node<T>, NodeData>();
            _edges = new HashSet<Edge<T>>();
            this.Name = "";
        }

        #endregion

        #region Public

        public string Name { get; set; }

        public Func<T, T> Cloner { get; set; }

        public Node<T> AddNode(T value)
        {
            var node = new Node<T>(this, value);
            var nodeData = new NodeData(node);

            _nodes.Add(node, nodeData);

            return node;
        }

        public IReadOnlyCollection<Node<T>> Nodes => _nodes.Keys;

        public IReadOnlyCollection<Edge<T>> Edges => _edges;

        public void RemoveNode(Node<T> node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            this.CheckNode(node);

            var data = _nodes[node];

            // deal with outgoing edges
            foreach (var outgoingEdge in data.OutgoingEdges)
            {
                var to = outgoingEdge.To;
                var dataTo = _nodes[to];

                var removed = dataTo.IncomingEdges.Remove(outgoingEdge);
                if (!removed)
                {
                    throw new GraphIntegrityViolationException();
                }

                removed = _edges.Remove(outgoingEdge);
                if (!removed)
                {
                    throw new GraphIntegrityViolationException();
                }
            }

            // deal with incoming edges
            foreach (var incomingEdge in data.IncomingEdges)
            {
                var from = incomingEdge.From;
                var dataFrom = _nodes[from];
                var removed = dataFrom.OutgoingEdges.Remove(incomingEdge);
                if (!removed)
                {
                    throw new GraphIntegrityViolationException();
                }

                removed = _edges.Remove(incomingEdge);
                if (!removed)
                {
                    throw new GraphIntegrityViolationException();
                }
            }

            // remove & invalidate node
            node.Graph = null;
            _nodes.Remove(node);
        }

        public void RemoveEdge(Edge<T> edge)
        {
            if (edge == null)
            {
                throw new ArgumentNullException(nameof(edge));
            }

            if (!_edges.Contains(edge))
            {
                throw new ArgumentException("Edge does not belong to the given graph", nameof(edge));
            }

            var from = edge.From;
            var to = edge.To;

            var valid =
                _nodes.ContainsKey(from) &&
                _nodes.ContainsKey(to) &&
                from.Graph == this &&
                to.Graph == this;

            if (!valid)
            {
                throw new GraphIntegrityViolationException();
            }

            var fromData = _nodes[from];
            var toData = _nodes[to];

            var removedFrom = fromData.OutgoingEdges.Remove(edge);
            var removedTo = toData.IncomingEdges.Remove(edge);

            if (!(removedFrom && removedTo))
            {
                throw new GraphIntegrityViolationException();
            }

            edge.From = null;
            edge.To = null;
            _edges.Remove(edge);
        }

        public Graph<T> Clone()
        {
            var clonedGraph = new Graph<T>();

            var nodesByTags = new Dictionary<int, Node<T>>();

            var map = new Dictionary<Node<T>, Node<T>>();

            foreach (var node in this.Nodes)
            {
                var clonedValue = this.Cloner(node.Value);
                var clonedNode = clonedGraph.AddNode(clonedValue);

                map.Add(node, clonedNode);
            }

            foreach (var edge in this.Edges)
            {
                var oldFrom = edge.From;
                var oldTo = edge.To;

                var clonedFrom = map[oldFrom];
                var clonedTo = map[oldTo];

                clonedFrom.DrawEdgeTo(clonedTo);
            }

            return clonedGraph;
        }

        public void CaptureNodes(IReadOnlyList<Node<T>> otherGraphNodes)
        {
            if (otherGraphNodes == null)
            {
                throw new ArgumentNullException(nameof(otherGraphNodes));
            }

            if (otherGraphNodes.Count == 0)
            {
                return;
            }

            Graph<T> otherGraph = null;
            var edgesToReview = new HashSet<Edge<T>>();

            for (var i = 0; i < otherGraphNodes.Count; i++)
            {
                var otherNode = otherGraphNodes[i];
                if (otherNode == null)
                {
                    throw new ArgumentException("Nodes cannot contain nulls", nameof(otherGraphNodes));
                }

                if (otherNode.Graph == null)
                {
                    throw new ArgumentException("Nodes cannot contain detached ones", nameof(otherGraphNodes));
                }

                if (otherNode.Graph == this)
                {
                    throw new ArgumentException("Cannot capture own nodes", nameof(otherGraphNodes));
                }

                if (i == 0)
                {
                    otherGraph = otherNode.Graph;
                }

                if (!ReferenceEquals(otherNode.Graph, otherGraph))
                {
                    throw new ArgumentException("Nodes must belong to a single graph", nameof(otherGraphNodes));
                }

                this.EnrollNode(otherNode, otherGraph._nodes[otherNode]);
                otherGraph.EvictNode(otherNode);

                edgesToReview.UnionWith(otherNode.OutgoingEdges);
                edgesToReview.UnionWith(otherNode.IncomingEdges);
            }

            foreach (var edgeToReview in edgesToReview)
            {
                var from = edgeToReview.From;
                var to = edgeToReview.To;

                var fromBelongsToMe = ReferenceEquals(from.Graph, this);
                var toBelongsToMe = ReferenceEquals(to.Graph, this);

                Trace.Assert(fromBelongsToMe || toBelongsToMe);

                if (fromBelongsToMe && toBelongsToMe)
                {
                    this.EnrollEdge(edgeToReview);
                    otherGraph.EvictEdge(edgeToReview);
                }
                else if (fromBelongsToMe)
                {
                    // 'from' belongs to me, 'to' doesn't
                    Trace.Assert(!toBelongsToMe);

                    var fromData = _nodes[from];
                    var deleted = fromData.OutgoingEdges.Remove(edgeToReview);
                    if (!deleted)
                    {
                        throw new GraphIntegrityViolationException(); // redundant check
                    }

                    var toData = otherGraph._nodes[to];
                    deleted = toData.IncomingEdges.Remove(edgeToReview);
                    if (!deleted)
                    {
                        throw new GraphIntegrityViolationException(); // redundant check
                    }

                    otherGraph.EvictEdge(edgeToReview);

                    // this edge disappears from 'other' graph and is not enrolled to 'me'
                    edgeToReview.From = null;
                    edgeToReview.To = null;
                }
                else
                {
                    // 'to' belongs to me, 'from' doesn't
                    Trace.Assert(!fromBelongsToMe); // todo1: replace all Trace.Assert with exceptions

                    var toData = _nodes[to];
                    var deleted = toData.IncomingEdges.Remove(edgeToReview);
                    if (!deleted)
                    {
                        throw new GraphIntegrityViolationException(); // redundant check
                    }

                    var fromData = otherGraph._nodes[from];
                    deleted = fromData.OutgoingEdges.Remove(edgeToReview);
                    if (!deleted)
                    {
                        throw new GraphIntegrityViolationException(); // redundant check
                    }

                    otherGraph.EvictEdge(edgeToReview);

                    // this edge disappears from 'other' graph and is not enrolled to 'me'
                    edgeToReview.From = null;
                    edgeToReview.To = null;
                }
            }
        }

        #endregion

        #region Internal

        internal Edge<T> DrawEdge(Node<T> from, Node<T> to)
        {
            // 'from' cannot be null by design

            if (to == null)
            {
                throw new ArgumentNullException(nameof(to));
            }

            this.CheckNode(to);

            var fromData = _nodes[from];
            var toData = _nodes[to];

            var edge = new Edge<T>(from, to);
            _edges.Add(edge);

            fromData.AddOutgoingEdge(edge);
            toData.AddIncomingEdge(edge);

            return edge;
        }

        internal IReadOnlyCollection<Edge<T>> GetOutgoingEdges(Node<T> node)
        {
            var nodeData = _nodes[node];
            return nodeData.OutgoingEdges;
        }

        internal IReadOnlyCollection<Edge<T>> GetIncomingEdges(Node<T> node)
        {
            var nodeData = _nodes[node];
            return nodeData.IncomingEdges;
        }

        #endregion

        #region Private

        private static T DefaultCloner(T value)
        {
            return value;
        }

        private void CheckNode(Node<T> node)
        {
            if (!ReferenceEquals(node.Graph, this))
            {
                throw new InvalidOperationException("Node does not belong to the given graph");
            }
        }

        private void EnrollNode(Node<T> otherNode, NodeData otherNodeData)
        {
            var valid =
                otherNode != null &&
                otherNode.Graph != null &&
                otherNode.Graph != this &&
                !_nodes.ContainsKey(otherNode) &&
                otherNodeData != null;

            if (!valid)
            {
                throw new GraphIntegrityViolationException(); // redundant check
            }

            _nodes.Add(otherNode, otherNodeData);
            otherNode.Graph = this;
        }

        private void EvictNode(Node<T> existingNode)
        {
            var valid =
                existingNode != null &&
                existingNode.Graph != null &&
                existingNode.Graph != this &&
                _nodes.ContainsKey(existingNode);

            if (!valid)
            {
                throw new GraphIntegrityViolationException(); // redundant check
            }

            _nodes.Remove(existingNode);
        }

        private void EnrollEdge(Edge<T> otherEdge)
        {
            var valid =
                otherEdge != null &&
                otherEdge.From.Graph == this &&
                otherEdge.To.Graph == this &&
                !_edges.Contains(otherEdge);

            if (!valid)
            {
                throw new GraphIntegrityViolationException(); // redundant check
            }

            _edges.Add(otherEdge);
        }

        private void EvictEdge(Edge<T> existingEdge)
        {
            var valid =
                existingEdge != null &&
                existingEdge.From != null &&
                existingEdge.To != null &&
                existingEdge.From.Graph != null &&
                existingEdge.To.Graph != null &&
                _edges.Contains(existingEdge);

            if (!valid)
            {
                throw new GraphIntegrityViolationException(); // redundant check
            }

            _edges.Remove(existingEdge);
        }

        #endregion
    }
}
