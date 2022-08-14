using BehaviorTree.Abstract;

namespace BehaviorTree.LeafNodes
{
    public sealed class ActionNode : Leaf
    {
        public delegate STATUS ProcessNode();
        public event ProcessNode Process;

        public ActionNode(string name, ProcessNode process) : base(name)
        {
            type = TYPE.ACTION;
            Process += process;
        }

        public override STATUS Tick()
        {
            SetStats();

            var action = Process?.Invoke();
            if (action.HasValue)
                status = action.Value;
            else
                status = STATUS.FAILURE;

            return status;
        }
    }
}
