using BehaviorTree.Abstract;

namespace BehaviorTree.CompositeNodes
{
    public sealed class SyncParallelNode : Composite
    {
        public enum POLICY { REQUIRE_ONE, REQUIRE_ALL }

        private readonly POLICY policySuccess;
        private readonly POLICY policyFailure;
        private readonly STATUS stateWhenInInfiniteLoop;

        private int successCount;
        private int failureCount;

        public SyncParallelNode(string name, POLICY success, POLICY failure, STATUS stateWhenInInfiniteLoop) : base(name)
        {
            policySuccess = success;
            policyFailure = failure;
            this.stateWhenInInfiniteLoop = stateWhenInInfiniteLoop;
        }

        public override STATUS Tick()
        {
            SetStats();

            var filterChildren = children.Where(x => x.State != STATE.FINISHED).ToList();
            if (filterChildren.Any())
            {
                foreach (var child in filterChildren)
                {
                    var action = child.Tick();

                    if (action == STATUS.SUCCESS)
                    {
                        child.SetState(STATE.FINISHED);

                        successCount++;
                        if (policySuccess == POLICY.REQUIRE_ONE)
                        {
                            status = STATUS.SUCCESS;
                            break;
                        }
                    }
                    else if (action == STATUS.FAILURE)
                    {
                        child.SetState(STATE.FINISHED);

                        failureCount++;
                        if (policyFailure == POLICY.REQUIRE_ONE)
                        {
                            status = STATUS.FAILURE;
                            break;
                        }
                    }
                }
            }
            else
                status = stateWhenInInfiniteLoop;

            if (policyFailure == POLICY.REQUIRE_ALL && failureCount == children.Count)
                status = STATUS.FAILURE;

            if (policySuccess == POLICY.REQUIRE_ALL && successCount == children.Count)
                status = STATUS.SUCCESS;

            if (status != STATUS.RUNNING)
                children.ForEach(x => x.Reset());

            return status;
        }

        public override void SetStats()
        {
            if (state == STATE.IDLE)
            {
                successCount = failureCount = 0;
                state = STATE.WORKING;
            }

            status = STATUS.RUNNING;
        }
    }
}
