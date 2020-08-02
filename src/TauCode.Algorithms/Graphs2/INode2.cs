using System.Collections.Generic;

namespace TauCode.Algorithms.Graphs2
{
    public interface INode2<T>
    {
        T Value { get; set; }

        IEdge2<T> DrawEdgeTo(INode2<T> another);

        IReadOnlyCollection<IEdge2<T>> OutgoingEdges { get; }

        IReadOnlyCollection<IEdge2<T>> IncomingEdges { get; }
    }
}
