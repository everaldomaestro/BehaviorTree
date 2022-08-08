using BehaviorTree.Abstract;

namespace BehaviorTree.DecoratorNodes
{
    public sealed class RetryNode : Decorator
    {
        private readonly int times;
        int countFailure = 0;

        public RetryNode(string name, int times) : base(name) { this.times = times; }

        public override STATUS Tick()
        {
            SetStats();

            var action = children[currentChild].Tick();
            if (action == STATUS.FAILURE)
                countFailure++;

            if (action == STATUS.SUCCESS)
            {
                children[currentChild].Reset();
                status = STATUS.SUCCESS;
                countFailure = 0;
            }
            else if (countFailure >= times)
            {
                children[currentChild].Reset();
                status = action;
                countFailure = 0;
            }

            return status;
        }
    }
}
