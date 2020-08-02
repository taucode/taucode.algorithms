using System.Collections.Generic;

namespace TauCode.Algorithms.Graphs
{
    public interface INode<T> : IPropertyOwner
    {
        IGraph<T> Graph { get; }

        T Value { get; set; }

        IReadOnlyList<IEdge<T>> OutgoingEdges { get; }

        IReadOnlyList<IEdge<T>> IncomingEdges { get; }
    }
}
