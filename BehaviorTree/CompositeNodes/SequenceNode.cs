using BehaviorTree.Abstract;

namespace BehaviorTree.CompositeNodes
{
    public sealed class SequenceNode : Composite
    {
        public SequenceNode(string name) : base(name) { }

        public override STATUS Tick()
        {
            SetStats();

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

            return status;
        }
    }
}
