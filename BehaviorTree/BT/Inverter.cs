namespace BehaviorTree.BT
{
    public class Inverter : Node
    {
        public Inverter(string name) : base(name)
        {
            type = TYPE.DECORATOR;
        }

        public override STATUS Process()
        {
            status = STATUS.RUNNING;

            var action = childrens[currentChild].Process();

            if (action == STATUS.SUCCESS)
            {
                status = STATUS.FAILURE;
            }
            else if (action == STATUS.FAILURE)
            {
                status = STATUS.SUCCESS;
            }

            Console.WriteLine($"{name} - {status}");
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
