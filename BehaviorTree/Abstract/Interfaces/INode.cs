using static BehaviorTree.Abstract.Node;

namespace BehaviorTree.Abstract.Interfaces
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