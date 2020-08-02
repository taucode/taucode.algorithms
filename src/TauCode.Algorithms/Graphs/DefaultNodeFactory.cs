using System;

namespace TauCode.Algorithms.Graphs
{
    internal class DefaultNodeFactory<T> : INodeFactory<T>
    {
        private readonly Graph<T> _graph;

        internal DefaultNodeFactory(Graph<T> graph)
        {
            _graph = graph ?? throw new ArgumentNullException(nameof(graph));
        }

        public INode<T> CreateNode(T value)
        {
            return new Node<T>(_graph, value);
        }
    }
}
