using BehaviorTree.Abstract;

namespace BehaviorTree.CompositeNodes
{
    public sealed class FallbackNode : Composite
    {
        public FallbackNode(string name) : base(name) { }

        public override STATUS Tick()
        {
            SetStats();

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

                if (IsTheLastChild())
                {
                    currentChild = 0;
                    status = STATUS.FAILURE;
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
