using BehaviorTree.BT.Abstract;

namespace BehaviorTree.BT.DecoratorNodes
{
    public sealed class InverterNode : Decorator
    {
        public InverterNode(string name) : base(name) { }

        public override STATUS Tick()
        {
            SetStats();

            var action = children[currentChild].Tick();
            if (action == STATUS.SUCCESS)
            {
                children[currentChild].Reset();
                status = STATUS.FAILURE;
            }
            else if (action == STATUS.FAILURE)
            {
                children[currentChild].Reset();
                status = STATUS.SUCCESS;
            }

            return status;
        }
    }
}
