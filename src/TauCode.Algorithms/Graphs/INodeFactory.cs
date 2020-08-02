namespace TauCode.Algorithms.Graphs
{
    public interface INodeFactory<T>
    {
        INode<T> CreateNode(T value);
    }
}
