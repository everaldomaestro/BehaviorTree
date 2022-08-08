using BehaviorTree.BT.Abstract;

namespace BehaviorTree.BT.DecoratorNodes
{
    public sealed class ForceFailureNode : Decorator
    {
        public ForceFailureNode(string name) : base(name) { }

        public override STATUS Tick()
        {
            SetStats();

            var action = children[currentChild].Tick();
            if (action != STATUS.RUNNING)
            {
                children[currentChild].Reset();
                status = STATUS.FAILURE;
            }

            return status;
        }
    }
}
