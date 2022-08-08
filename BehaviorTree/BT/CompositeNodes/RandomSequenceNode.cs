using BehaviorTree.BT.Abstract;

namespace BehaviorTree.BT.CompositeNodes
{
    public sealed class RandomSequenceNode : RandomComposite
    {
        public RandomSequenceNode(string name) : base(name) { }

        public override STATUS Tick()
        {
            if (state == STATE.IDLE)
            {
                state = STATE.WORKING;

                if (!ChildTickRandom())
                {
                    status = STATUS.FAILURE;
                    return status;
                }
            }

            status = STATUS.RUNNING;

            var action = children[currentChild].Tick();
            if (action == STATUS.FAILURE)
            {
                status = STATUS.FAILURE;
            }
            else if (action == STATUS.SUCCESS)
            {
                children[currentChild].SetState(STATE.FINISHED);

                if (!ChildTickRandom())
                    status = STATUS.SUCCESS;
                else
                    status = STATUS.RUNNING;
            }

            if (status != STATUS.RUNNING)
                children.ForEach(x => x.Reset());

            return status;
        }
    }
}
