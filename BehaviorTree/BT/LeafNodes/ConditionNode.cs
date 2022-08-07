namespace BehaviorTree.BT.LeafNodes
{
    public sealed class ConditionNode : Node
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
            if (state == STATE.IDLE)
                state = STATE.EXECUTING;

            status = STATUS.RUNNING;

            var action = Process?.Invoke();

            if (action == STATUS.SUCCESS)
                status = STATUS.SUCCESS;
            else
                status = STATUS.FAILURE;

            Console.WriteLine($"{name} - {status}");
            return status;
        }
    }
}
