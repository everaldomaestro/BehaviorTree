using BehaviorTree.Abstract;

namespace BehaviorTree.CompositeNodes
{
    public sealed class RandomFallbackNode : RandomComposite
    {
        public RandomFallbackNode(string name) : base(name) { }

        public override STATUS Tick()
        {
            SetStats();
            if (status == STATUS.FAILURE)
                return status;

            status = STATUS.RUNNING;

            var action = children[currentChild].Tick();
            if (action == STATUS.SUCCESS)
            {
                status = STATUS.SUCCESS;
            }
            else if (action == STATUS.FAILURE)
            {
                children[currentChild].SetState(STATE.FINISHED);

                if (!ChildTickRandom())
                    status = STATUS.FAILURE;
                else
                    status = STATUS.RUNNING;
            }

            if (status != STATUS.RUNNING)
                children.ForEach(x => x.Reset());

            return status;
        }
    }
}
