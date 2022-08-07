namespace BehaviorTree.BT.LeafNodes
{
    public sealed class ActionNode : Node
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
            if (state == STATE.IDLE)
                state = STATE.EXECUTING;

            status = STATUS.RUNNING;

            var action = Process?.Invoke();

            if (action.HasValue)
                status = action.GetValueOrDefault();
            else
                status = STATUS.FAILURE;

            Console.WriteLine($"{name} - {status}");
            return status;
        }
    }
}
