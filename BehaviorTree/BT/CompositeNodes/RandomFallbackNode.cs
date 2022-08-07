using BehaviorTree.BT.Interfaces;

namespace BehaviorTree.BT.CompositeNodes
{
    public sealed class RandomFallbackNode : Node, IParentNode
    {
        public RandomFallbackNode(string name) : base(name)
        {
            type = TYPE.COMPOSITE;
        }

        public override STATUS Tick()
        {
            if (state == STATE.IDLE)
            {
                if (!ChildTickRandom())
                    return STATUS.FAILURE;

                state = STATE.EXECUTING;
            }

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

            Console.WriteLine($"{name} - {status}");
            return status;
        }

        private bool ChildTickRandom()
        {
            var idleChildren = children.Where(x => x.State == STATE.IDLE).ToList();
            if (!idleChildren.Any())
                return false;

            var randomIndex = new Random().Next(0, idleChildren.Count - 1);
            currentChild = children.IndexOf(idleChildren[randomIndex]);

            return true;
        }
    }
}
