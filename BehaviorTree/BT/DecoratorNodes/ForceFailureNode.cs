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
                children[currentChild].SetState(STATE.IDLE);
                status = STATUS.FAILURE;
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
