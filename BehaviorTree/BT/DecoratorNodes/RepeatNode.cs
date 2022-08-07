using BehaviorTree.BT.Interfaces;

namespace BehaviorTree.BT.DecoratorNodes
{
    public sealed class RepeatNode : Node, IParentNode
    {
        private readonly int times;
        int countSuccess = 0;

        public RepeatNode(string name, int times) : base(name)
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
            if (action == STATUS.SUCCESS)
                countSuccess++;

            if (action == STATUS.FAILURE)
            {
                children[currentChild].Reset();
                status = STATUS.FAILURE;
                countSuccess = 0;
            }
            else if (countSuccess >= times)
            {
                children[currentChild].Reset();
                status = STATUS.SUCCESS;
                countSuccess = 0;
            }

            Console.WriteLine($"{name} - {status}");
            return status;
        }
    }
}
