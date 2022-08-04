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
                children[currentChild].SetState(STATE.IDLE);
                status = STATUS.SUCCESS;
            }
            else if (countFailure >= times)
            {
                children[currentChild].SetState(STATE.IDLE);
                status = action;
            }

            Console.WriteLine($"{name} - {status}");
            return status;
        }

        public override void Attach(Node child)
        {
            if (HasChildren())
                throw new InvalidOperationException($"Unable to add Node {child}.");

            base.Attach(child);
        }
    }
}
