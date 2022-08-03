namespace BehaviorTree.BT
{
    public class Retry : Node
    {
        private readonly int num_attempts;
        int count = 0;

        public Retry(string name, int num_attempts) : base(name)
        {
            type = TYPE.DECORATOR;
            this.num_attempts = num_attempts;
        }

        public override STATUS Process()
        {
            status = STATUS.RUNNING;

            var action = childrens[currentChild].Process();
            count++;

            if (action == STATUS.SUCCESS)
            {
                status = STATUS.SUCCESS;
            }
            else if (action == STATUS.FAILURE && count >= num_attempts)
            {
                status = STATUS.FAILURE;
            }

            Console.WriteLine($"{name} - c({count})- {status}");
            return status;
        }

        public override void AddChild(Node child)
        {
            if (childrens.Count == 0)
                childrens.Add(child);
            else
                throw new FormatException($"Unable to add Node {child}.");
        }
    }
}
