namespace TauCode.Algorithms.Graphs
{
    public interface IEdge<T>
    {
        INode<T> From { get; }
        INode<T> To { get; }
        void Disappear();
    }
}
