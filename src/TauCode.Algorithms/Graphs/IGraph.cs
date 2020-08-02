using System.Collections.Generic;

namespace TauCode.Algorithms.Graphs
{
    public interface IGraph<T>
    {
        void AddNode(INode<T> node);

        bool ContainsNode(INode<T> node);

        bool RemoveNode(INode<T> node);

        IReadOnlyCollection<INode<T>> Nodes { get; }
    }
}
