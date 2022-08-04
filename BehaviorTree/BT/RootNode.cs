using BehaviorTree.BT.Interfaces;

namespace BehaviorTree.BT
{
    public sealed class RootNode : Node, IParentNode
    {
        private readonly bool executeInLoop;
        public bool ExecuteInLoop => executeInLoop;

        public RootNode(string name, bool executeInLoop = false) : base(name)
        {
            this.executeInLoop = executeInLoop;
            type = TYPE.COMPOSITE;
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
                children[currentChild].SetState(STATE.IDLE);
                status = action;
            }

            Console.WriteLine($"{name} - {status}");
            return status;
        }

        public override void Attach(Node child)
        {
            if (HasChildren() || child.Type != TYPE.COMPOSITE)
                throw new InvalidOperationException($"Unable to add Node {child}.");

            base.Attach(child);
        }
    }
}
