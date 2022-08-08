using BehaviorTree.BT.Abstract;

namespace BehaviorTree.BT.Interfaces
{
    public interface IParentNode : INode
    {
        void Attach(Node child);
        void Detach(Node child);
    }
}