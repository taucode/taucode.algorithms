using System.Collections.Generic;

namespace TauCode.Algorithms.Graphs2
{
    public interface IGraph2<T>
    {
        void AddNode(INode2<T> node);

        bool ContainsNode(INode2<T> node);

        bool RemoveNode(INode2<T> node);

        IReadOnlyCollection<INode2<T>> Nodes { get; }
    }
}
