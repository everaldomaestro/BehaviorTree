namespace BehaviorTree.BT
{
    public class Sequence : Node
    {
        public Sequence(string name) : base(name)
        {
            type = TYPE.CONTROL;
        }

        public override STATUS Process()
        {
            status = STATUS.RUNNING;

            var action = childrens[currentChild].Process();
            if (action == STATUS.FAILURE)
            {
                currentChild = 0;
                status = STATUS.FAILURE;
            }
            else if (action == STATUS.SUCCESS)
            {
                if (currentChild == childrens.Count - 1)
                {
                    currentChild = 0;
                    status = STATUS.SUCCESS;
                }
                else
                {
                    currentChild++;
                }
            }

            Console.WriteLine($"{name} - {status}");
            return status;
        }
    }
}
