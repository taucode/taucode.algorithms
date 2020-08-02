using System;
using System.Collections.Generic;

namespace TauCode.Algorithms.Graphs
{
    public interface IGraph<T> : IPropertyOwner
    {
        string Name { get; set; }

        Func<T, T> Cloner { get; set; }

        INodeFactory<T> NodeFactory { get; set; }

        INode<T> AddNode(T value);

        void RemoveNode(INode<T> node);

        IEdge<T> DrawEdge(INode<T> from, INode<T> to);

        public void RemoveEdge(IEdge<T> edge);

        IReadOnlyList<INode<T>> Nodes { get; }

        IReadOnlyList<IEdge<T>> Edges { get; }

        Graph<T> Clone();

        void CaptureNodes(IReadOnlyList<INode<T>> otherGraphNodes);
    }
}
