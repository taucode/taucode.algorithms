using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TauCode.Algorithms.Graphs
{
    [DebuggerDisplay("{" + nameof(Value) + "}")]
    internal class Node<T> : INode<T>
    {
        #region Fields

        private Graph<T> _graph;

        #endregion

        #region Constructor

        internal Node(Graph<T> graph, T value)
        {
            this.Graph = _graph;
            this.Value = value;
        }

        #endregion

        #region INode<T> Members

        public IGraph<T> Graph { get; internal set; }

        public T Value { get; set; }

        public IReadOnlyList<IEdge<T>> OutgoingEdges
        {
            get
            {
                throw new NotImplementedException();
                //this.CheckIsAttached();
                //return this.Graph.GetOutgoingEdges(this);
            }
        }

        public IReadOnlyList<IEdge<T>> IncomingEdges
        {
            get
            {
                throw new NotImplementedException();
                //this.CheckIsAttached();
                //return this.Graph.GetIncomingEdges(this);
            }
        }

        #endregion

        #region IPropertyOwner Members

        public void SetProperty(string propertyName, object propertyValue)
        {
            throw new NotImplementedException();
        }

        public object GetProperty(string propertyName)
        {
            throw new NotImplementedException();
        }

        public bool HasProperty(string propertyName)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<string> PropertyNames => throw new NotImplementedException();

        #endregion





        //public Edge<T> DrawEdgeTo(Node<T> other)
        //{
        //    this.CheckIsAttached();
        //    return this.Graph.DrawEdge(this, other);
        //}


        private void CheckIsAttached()
        {
            if (this.Graph == null)
            {
                throw new InvalidOperationException("Node is detached");
            }
        }
    }
}
