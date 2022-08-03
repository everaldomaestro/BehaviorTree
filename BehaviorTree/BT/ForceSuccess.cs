﻿namespace BehaviorTree.BT
{
    public class ForceSuccess : Node
    {
        public ForceSuccess(string name) : base(name)
        {
            type = TYPE.DECORATOR;
        }

        public override STATUS Process()
        {
            status = STATUS.RUNNING;

            var action = childrens[currentChild].Process();

            if (action != STATUS.RUNNING)
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