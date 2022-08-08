﻿using BehaviorTree.BT.Abstract;

namespace BehaviorTree.BT.CompositeNodes
{
    public sealed class FallbackNode : Composite
    {
        public FallbackNode(string name) : base(name) { }

        public override STATUS Tick()
        {
            SetStats();

            var action = children[currentChild].Tick();
            if (action == STATUS.SUCCESS)
            {
                status = STATUS.SUCCESS;
                children[currentChild].Reset();
                currentChild = 0;
            }
            else if (action == STATUS.FAILURE)
            {
                children[currentChild].Reset();

                if (currentChild == children.Count - 1)
                {
                    currentChild = 0;
                    status = STATUS.FAILURE;
                }
                else
                {
                    currentChild++;
                }
            }

            return status;
        }
    }
}
