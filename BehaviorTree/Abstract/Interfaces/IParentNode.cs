namespace BehaviorTree.Abstract.Interfaces
{
    public interface IParentNode : INode
    {
        void Attach(Node child);
        void Detach(Node child);
    }
}