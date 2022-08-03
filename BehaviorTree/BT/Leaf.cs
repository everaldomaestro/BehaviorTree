namespace BehaviorTree.BT
{
    public class Leaf : Node
    {
        public delegate STATUS ProcessNode();
        public event ProcessNode Action;

        public Leaf(string name, TYPE type, ProcessNode process) : base(name)
        {
            this.type = type;
            Action += process;
        }

        public override STATUS Process()
        {
            status = STATUS.RUNNING;

            var action = Action?.Invoke();

            if (type == TYPE.ACTION)
            {
                status = action.GetValueOrDefault();
            }
            else
            {
                if (action == STATUS.SUCCESS)
                {
                    status = STATUS.SUCCESS;
                }
                else
                {
                    status = STATUS.FAILURE;
                }
            }

            Console.WriteLine($"{name} - {status}");
            return status;
        }

        public override void AddChild(Node child)
        {
            throw new FormatException($"Unable to add Node {child}.");
        }
    }
}
