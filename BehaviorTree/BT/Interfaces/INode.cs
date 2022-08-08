using BehaviorTree.BT.Abstract;
using static BehaviorTree.BT.Abstract.Node;

namespace BehaviorTree.BT.Interfaces
{
    public interface INode
    {
        STATUS Tick();
        STATUS GetStatus();
        STATE SetState(STATE state);
        void SetParent(Node child, bool removeParent = false);
        bool HasChildren();
        void Reset();
    }
}