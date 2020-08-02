namespace TauCode.Algorithms.Graphs2
{
    public interface IEdge2<T>
    {
        INode2<T> From { get; }
        INode2<T> To { get; }
        void Disappear();
    }
}
