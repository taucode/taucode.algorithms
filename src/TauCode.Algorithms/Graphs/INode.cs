using System.Collections.Generic;

namespace TauCode.Algorithms.Graphs
{
    public interface INode<T>
    {
        T Value { get; set; }

        IEdge<T> DrawEdgeTo(INode<T> another);

        IReadOnlyCollection<IEdge<T>> OutgoingEdges { get; }

        IReadOnlyCollection<IEdge<T>> IncomingEdges { get; }
    }
}
