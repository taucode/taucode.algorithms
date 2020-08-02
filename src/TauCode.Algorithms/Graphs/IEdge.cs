namespace TauCode.Algorithms.Graphs
{
    public interface IEdge<T> : IPropertyOwner
    {
        Node<T> From { get; }
        Node<T> To { get; }
    }
}
