namespace BehaviorTree.BT
{
    public class Repeat : Node
    {
        private readonly int num_attempts;
        int count = 0;

        public Repeat(string name, int num_attempts) : base(name)
        {
            type = TYPE.DECORATOR;
            this.num_attempts = num_attempts;
        }

        public override STATUS Process()
        {
            status = STATUS.RUNNING;

            var action = childrens[currentChild].Process();
            if (action == STATUS.SUCCESS)
                count++;

            if (action == STATUS.FAILURE)
            {
                status = STATUS.FAILURE;
            }
            else if (count >= num_attempts)
            {
                status = action;
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
