using BehaviorTree.BT.Interfaces;

namespace BehaviorTree.BT.DecoratorNodes
{
    public sealed class RetryNode : Node, IParentNode
    {
        private readonly int times;
        int countFailure = 0;

        public RetryNode(string name, int times) : base(name)
        {
            type = TYPE.DECORATOR;
            this.times = times;
        }

        public override STATUS Tick()
        {
            if (state == STATE.IDLE)
                state = STATE.EXECUTING;

            status = STATUS.RUNNING;

            var action = children[currentChild].Tick();
            if (action == STATUS.FAILURE)
                countFailure++;

            if (action == STATUS.SUCCESS)
            {
                children[currentChild].Reset();
                status = STATUS.SUCCESS;
                countFailure = 0;
            }
            else if (countFailure >= times)
            {
                children[currentChild].Reset();
                status = action;
                countFailure = 0;
            }

            Console.WriteLine($"{name} - {status}");
            return status;
        }
    }
}
