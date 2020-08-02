namespace TauCode.Algorithms.Graphs
{
    public interface IEdge<T>
    {
        INode2<T> From { get; }
        INode2<T> To { get; }
        void Disappear();
    }
}
