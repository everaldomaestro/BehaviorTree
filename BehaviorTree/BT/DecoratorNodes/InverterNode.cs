using BehaviorTree.BT.Interfaces;

namespace BehaviorTree.BT.DecoratorNodes
{
    public sealed class InverterNode : Node, IParentNode
    {
        public InverterNode(string name) : base(name)
        {
            type = TYPE.DECORATOR;
        }

        public override STATUS Tick()
        {
            if (state == STATE.IDLE)
                state = STATE.EXECUTING;

            status = STATUS.RUNNING;

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

            Console.WriteLine($"{name} - {status}");
            return status;
        }
    }
}
