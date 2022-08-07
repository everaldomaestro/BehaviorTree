using BehaviorTree.BT.Interfaces;

namespace BehaviorTree.BT.CompositeNodes
{
    public sealed class FallbackNode : Node, IParentNode
    {
        public FallbackNode(string name) : base(name)
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
                children[currentChild].Reset();
                currentChild = 0;
            }
            else if (action == STATUS.FAILURE)
            {
                children[currentChild].Reset();

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
