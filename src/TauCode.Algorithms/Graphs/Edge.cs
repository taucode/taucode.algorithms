using System;
using System.Collections.Generic;
using System.Diagnostics;

// todo clean up
// todo ut properties functionality, for all classes IPropertiesOwner
namespace TauCode.Algorithms.Graphs
{
    [DebuggerDisplay("{From.Value} -> {To.Value}")]
    internal class Edge<T> : IEdge<T>
    {
        //private readonly Dictionary<string, object> _properties;

        #region Constructor

        internal Edge(INode<T> from, INode<T> to)
        {
            // 'from' and 'to' cannot be nulls by design

            this.From = from;
            this.To = to;

            //_properties = new Dictionary<string, object>();
        }

        #endregion

        #region IEdge<T> Members

        public INode<T> From { get; internal set; }

        public INode<T> To { get; internal set; }

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
    }
}
