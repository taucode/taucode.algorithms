using System;
using System.Collections.Generic;

namespace TauCode.Algorithms.Graphs
{
    public class Node2<T> : INode2<T>
    {
        #region Fields

        private readonly HashSet<Edge2<T>> _outgoingEdges;
        private readonly HashSet<Edge2<T>> _incomingEdges;

        #endregion

        #region Constructor

        public Node2(T value)
        {
            this.Value = value;

            _outgoingEdges = new HashSet<Edge2<T>>();
            _incomingEdges = new HashSet<Edge2<T>>();
        }

        #endregion

        #region INode2<T> Members

        public T Value { get; set; }

        public IEdge<T> DrawEdgeTo(INode2<T> another)
        {
            if (another == null)
            {
                throw new ArgumentNullException(nameof(another));
            }

            var castedAnother = another as Node2<T>;
            if (castedAnother == null)
            {
                throw new ArgumentException($"Expected node of type '{typeof(Node2<T>).FullName}'.", nameof(another));
            }
            
            var edge = new Edge2<T>(this, castedAnother);

            this._outgoingEdges.Add(edge);
            castedAnother._incomingEdges.Add(edge);

            return edge;
        }

        public IReadOnlyCollection<IEdge<T>> OutgoingEdges => _outgoingEdges;

        public IReadOnlyCollection<IEdge<T>> IncomingEdges => _incomingEdges;

        #endregion
    }
}
