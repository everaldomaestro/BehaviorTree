using BehaviorTree.BT.Interfaces;

namespace BehaviorTree.BT.DecoratorNodes
{
    public sealed class ForceFailureNode : Node, IParentNode
    {
        public ForceFailureNode(string name) : base(name)
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
                status = STATUS.FAILURE;
            }

            Console.WriteLine($"{name} - {status}");
            return status;
        }
    }
}
