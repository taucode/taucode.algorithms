using System.Collections.Generic;

namespace TauCode.Algorithms.Graphs
{
    public interface INode2<T>
    {
        T Value { get; set; }

        IEdge<T> DrawEdgeTo(INode2<T> another);

        IReadOnlyCollection<IEdge<T>> OutgoingEdges { get; }

        IReadOnlyCollection<IEdge<T>> IncomingEdges { get; }
    }
}
