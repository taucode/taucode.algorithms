using System;
using System.Collections.Generic;

namespace TauCode.Algorithms.Graphs2
{
    public class Graph2<T> : IGraph2<T>
    {
        #region Fields

        private readonly HashSet<INode2<T>> _nodes;

        #endregion

        #region Constructor

        public Graph2()
        {
            _nodes = new HashSet<INode2<T>>();
        }

        #endregion

        #region IGraph2<T> Members

        public void AddNode(INode2<T> node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            if (_nodes.Contains(node))
            {
                throw new ArgumentException("Graph already contains this node.", nameof(node));
            }

            _nodes.Add(node);
        }

        public bool ContainsNode(INode2<T> node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            return _nodes.Contains(node);
        }

        public bool RemoveNode(INode2<T> node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            var removed = _nodes.Remove(node);

            return removed;
        }

        public IReadOnlyCollection<INode2<T>> Nodes => _nodes;

        #endregion
    }
}
