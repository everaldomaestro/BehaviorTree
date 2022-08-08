using BehaviorTree.BT.Abstract;

namespace BehaviorTree.BT.DecoratorNodes
{
    public sealed class RepeatNode : Decorator
    {
        private readonly int times;
        int countSuccess = 0;

        public RepeatNode(string name, int times) : base(name) { this.times = times; }

        public override STATUS Tick()
        {
            SetStats();

            var action = children[currentChild].Tick();
            if (action == STATUS.SUCCESS)
                countSuccess++;

            if (action == STATUS.FAILURE)
            {
                children[currentChild].Reset();
                status = STATUS.FAILURE;
                countSuccess = 0;
            }
            else if (countSuccess >= times)
            {
                children[currentChild].Reset();
                status = STATUS.SUCCESS;
                countSuccess = 0;
            }

            return status;
        }
    }
}
