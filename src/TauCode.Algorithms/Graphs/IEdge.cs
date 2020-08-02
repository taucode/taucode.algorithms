namespace TauCode.Algorithms.Graphs
{
    public interface IEdge<T> : IPropertyOwner
    {
        INode<T> From { get; }
        INode<T> To { get; }
    }
}
