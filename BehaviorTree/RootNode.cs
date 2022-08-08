using BehaviorTree.Abstract;
using BehaviorTree.Abstract.Interfaces;

namespace BehaviorTree
{
    public sealed class RootNode : Node, IParentNode
    {
        private readonly bool executeInLoop;
        public bool ExecuteInLoop => executeInLoop;

        public RootNode(string name, bool executeInLoop = true) : base(name)
        {
            this.executeInLoop = executeInLoop;
            type = TYPE.ROOT;
        }

        public override STATUS Tick()
        {
            if (state == STATE.IDLE)
                state = STATE.WORKING;

            status = STATUS.RUNNING;

            var action = children[currentChild].Tick();

            if (!executeInLoop && action != STATUS.RUNNING)
            {
                SetState(STATE.IDLE);
                children[currentChild].Reset();
                status = action;
            }

            return status;
        }

        public void Attach(Node child)
        {
            if(HasChildren() || child.Type != TYPE.COMPOSITE)
               throw new InvalidOperationException($"Unable to add Node {child}.");

            SetParent(child);
            children.Add(child);
        }

        public void Detach(Node child)
        {
            SetParent(child, true);
            children.Remove(child);
        }
    }
}
