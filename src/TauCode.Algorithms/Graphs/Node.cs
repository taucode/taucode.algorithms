using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TauCode.Algorithms.Graphs
{
    [DebuggerDisplay("{" + nameof(Value) + "}")]
    public class Node<T>
    {
        internal Node(Graph<T> graph, T value)
        {
            this.Graph = graph;
            this.Value = value;
        }

        public Graph<T> Graph { get; internal set; }

        public T Value { get; }

        public Edge<T> DrawEdgeTo(Node<T> other)
        {
            this.CheckIsAttached();
            return this.Graph.DrawEdge(this, other);
        }

        public IReadOnlyCollection<Edge<T>> OutgoingEdges
        {
            get
            {
                this.CheckIsAttached();
                return this.Graph.GetOutgoingEdges(this);
            }
        }

        public IReadOnlyCollection<Edge<T>> IncomingEdges
        {
            get
            {
                this.CheckIsAttached();
                return this.Graph.GetIncomingEdges(this);
            }
        }

        private void CheckIsAttached()
        {
            if (this.Graph == null)
            {
                throw new InvalidOperationException("Node is detached");
            }
        }
    }
}
