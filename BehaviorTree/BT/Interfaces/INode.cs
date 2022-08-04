using static BehaviorTree.BT.Node;

namespace BehaviorTree.BT.Interfaces
{
    public interface INode
    {
        STATUS GetStatus();
        bool HasChildren();
        STATE SetState(STATE state);
        STATUS Tick();
        string ToString();
    }
}