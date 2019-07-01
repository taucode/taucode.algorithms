using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TauCode.Algorithms.Graphs
{
    [DebuggerDisplay("{From.Value} -> {To.Value}")]
    public class Edge<T>
    {
        private readonly Dictionary<string, object> _properties;

        internal Edge(Node<T> from, Node<T> to)
        {
            // 'from' and 'to' cannot be nulls by design

            this.From = from;
            this.To = to;

            _properties = new Dictionary<string, object>();
        }

        public Node<T> From { get; internal set; }
        public Node<T> To { get; internal set; }

        // todo1[ak] ut properties funcitonality

        public void SetProperty(string propertyName, object propertyValue)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            _properties[propertyName] = propertyValue;
        }

        public object GetProperty(string propertyName)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            if (!_properties.ContainsKey(propertyName))
            {
                throw new KeyNotFoundException($"Property '{propertyName}' not found");
            }

            return _properties[propertyName];
        }

        public TProperty GetProperty<TProperty>(string propertyName)
        {
            return (TProperty)GetProperty(propertyName);
        }

        public bool HasProperty(string propertyName)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            return _properties.ContainsKey(propertyName);
        }

        public IReadOnlyCollection<string> PropertyNames => _properties.Keys;
    }
}
