using BehaviorTree.Abstract;

namespace BehaviorTree.LeafNodes
{
    public sealed class ConditionNode : Leaf
    {
        public delegate STATUS ProcessNode();
        public event ProcessNode Process;

        public ConditionNode(string name, ProcessNode process) : base(name)
        {
            type = TYPE.CONDITION;
            Process += process;
        }

        public override STATUS Tick()
        {
            SetStats();

            var action = Process?.Invoke();
            if (action.HasValue && action.Value == STATUS.SUCCESS)
                status = STATUS.SUCCESS;
            else
                status = STATUS.FAILURE;

            return status;
        }
    }
}
