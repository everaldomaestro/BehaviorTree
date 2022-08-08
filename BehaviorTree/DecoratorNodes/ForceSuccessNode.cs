using BehaviorTree.Abstract;

namespace BehaviorTree.DecoratorNodes
{
    public sealed class ForceSuccessNode : Decorator
    {
        public ForceSuccessNode(string name) : base(name) { }

        public override STATUS Tick()
        {
            SetStats();

            var action = children[currentChild].Tick();
            if (action != STATUS.RUNNING)
            {
                children[currentChild].Reset();
                status = STATUS.SUCCESS;
            }

            return status;
        }
    }
}
