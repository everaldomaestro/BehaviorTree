using BehaviorTree.BT.Interfaces;

namespace BehaviorTree.BT.CompositeNodes
{
    public sealed class SelectorNode : Node, IParentNode
    {
        public SelectorNode(string name) : base(name)
        {
            type = TYPE.COMPOSITE;
        }

        public override STATUS Tick()
        {
            if (state == STATE.IDLE)
                state = STATE.EXECUTING;

            status = STATUS.RUNNING;

            var action = children[currentChild].Tick();
            if (action == STATUS.SUCCESS)
            {
                status = STATUS.SUCCESS;
                children[currentChild].SetState(STATE.IDLE);
                currentChild = 0;
            }

            if (action == STATUS.FAILURE)
            {
                children[currentChild].SetState(STATE.IDLE);

                if (currentChild == children.Count - 1)
                {
                    currentChild = 0;
                    status = STATUS.FAILURE;
                }
                else
                {
                    currentChild++;
                }
            }

            Console.WriteLine($"{name} - {status}");
            return status;
        }
    }
}
