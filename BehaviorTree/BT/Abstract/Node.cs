using BehaviorTree.BT.Interfaces;

namespace BehaviorTree.BT.Abstract
{
    public abstract class Node : INode
    {
        public enum STATE { IDLE, WORKING, FINISHED }
        public enum STATUS { RUNNING, SUCCESS, FAILURE }
        public enum TYPE { ROOT, COMPOSITE, DECORATOR, CONDITION, ACTION }

        protected string name;
        protected int currentChild = 0;
        protected STATE state;
        protected STATUS status;
        protected TYPE type;

        protected Node? parent;
        protected List<Node> children = new();

        protected Node(string name)
        {
            this.name = name;
            parent = null;
            state = STATE.IDLE;
        }

        public STATE State { get { return state; } }
        public TYPE Type { get { return type; } }        

        public void SetParent(Node child, bool removeParent = false)
        {
            if (!removeParent)
                child.parent = this;
            else
                child.parent = null;
        }

        public virtual void SetStats()
        {
            if (state == STATE.IDLE)
                state = STATE.WORKING;

            status = STATUS.RUNNING;
        }

        public virtual STATUS Tick() => STATUS.FAILURE;
        public virtual void Reset() => state = STATE.IDLE;
        public STATE SetState(STATE state) => this.state = state;
        public STATUS GetStatus() => status;
        public bool HasChildren() => children.Any();
        public override string ToString() => name;
    }
}
