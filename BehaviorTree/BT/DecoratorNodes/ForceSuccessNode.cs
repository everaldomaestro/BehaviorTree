using BehaviorTree.BT.Interfaces;

namespace BehaviorTree.BT.DecoratorNodes
{
    public sealed class ForceSuccessNode : Node, IParentNode
    {
        public ForceSuccessNode(string name) : base(name)
        {
            type = TYPE.DECORATOR;
        }

        public override STATUS Tick()
        {
            if (state == STATE.IDLE)
                state = STATE.EXECUTING;

            status = STATUS.RUNNING;

            var action = children[currentChild].Tick();

            if (action != STATUS.RUNNING)
            {
                children[currentChild].Reset();
                status = STATUS.SUCCESS;
            }

            Console.WriteLine($"{name} - {status}");
            return status;
        }
    }
}
