namespace BehaviorTree.BT
{
    public class Fallback : Node
    {
        public Fallback(string name) : base(name)
        {
            type = TYPE.CONTROL;
        }

        private int currentChild;

        public override STATUS Process()
        {
            status = STATUS.RUNNING;

            var action = childrens[currentChild].Process();
            if (action == STATUS.SUCCESS)
            {
                currentChild = 0;
                status = STATUS.SUCCESS;
            }

            if (action == STATUS.FAILURE)
            {
                if (currentChild == childrens.Count - 1)
                {
                    currentChild = 0;
                    status = STATUS.FAILURE;
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
