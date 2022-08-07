using BehaviorTree.BT.Interfaces;

namespace BehaviorTree.BT
{
    public sealed class RootNode : Node, IParentNode
    {
        private readonly bool executeInLoop;
        public bool ExecuteInLoop => executeInLoop;

        public RootNode(string name, bool executeInLoop = true) : base(name)
        {
            this.executeInLoop = executeInLoop;
            type = TYPE.ROOT;
        }

        public override STATUS Tick()
        {
            if (state == STATE.IDLE)
                state = STATE.EXECUTING;

            status = STATUS.RUNNING;

            var action = children[currentChild].Tick();

            if (!executeInLoop && action != STATUS.RUNNING)
            {
                SetState(STATE.IDLE);
                children[currentChild].Reset();
                status = action;
            }

            Console.WriteLine($"{name} - {status}");
            return status;
        }
    }
}
