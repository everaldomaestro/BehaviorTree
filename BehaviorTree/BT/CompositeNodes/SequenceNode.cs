using BehaviorTree.BT.Interfaces;

namespace BehaviorTree.BT.CompositeNodes
{
    public sealed class SequenceNode : Node, IParentNode
    {
        public SequenceNode(string name) : base(name)
        {
            type = TYPE.COMPOSITE;
        }

        public override STATUS Tick()
        {
            if (state == STATE.IDLE)
                state = STATE.EXECUTING;

            status = STATUS.RUNNING;

            var action = children[currentChild].Tick();
            if (action == STATUS.FAILURE)
            {
                children[currentChild].Reset();
                status = STATUS.FAILURE;
                currentChild = 0;
            }
            else if (action == STATUS.SUCCESS)
            {
                children[currentChild].Reset();

                if (currentChild == children.Count - 1)
                {
                    currentChild = 0;
                    status = STATUS.SUCCESS;
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
