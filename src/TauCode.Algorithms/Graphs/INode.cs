using System.Collections.Generic;

namespace TauCode.Algorithms.Graphs
{
    public interface INode<T>
    {
        IGraph<T> Graph { get; }

        T Value { get; set; }

        IReadOnlyList<Edge<T>> OutgoingEdges { get; }

        IReadOnlyList<Edge<T>> IncomingEdges { get; }
    }
}
