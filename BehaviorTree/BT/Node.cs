using BehaviorTree.BT.Interfaces;

namespace BehaviorTree.BT
{
    public abstract class Node : INode
    {
        public enum STATE { IDLE, EXECUTING, FINISHED }
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

        public virtual STATUS Tick() => STATUS.FAILURE;

        public virtual void Attach(Node child)
        {
            if (type == TYPE.ROOT && (HasChildren() || child.Type != TYPE.COMPOSITE))
                throw new InvalidOperationException($"Unable to add Node {child}.");
            else if(type == TYPE.ACTION || type == TYPE.CONDITION)
                throw new InvalidOperationException($"Unable to add Node {child}.");
            else if(type == TYPE.DECORATOR && HasChildren())
                throw new InvalidOperationException($"Unable to add Node {child}.");

            child.parent = this;
            children.Add(child);
        }

        public virtual void Detach(Node child)
        {
            child.parent = null;
            children.Remove(child);
        }

        public virtual void Reset()
        {
            state = STATE.IDLE;
        }

        public STATE SetState(STATE state) => this.state = state;
        public STATUS GetStatus() => status;
        public bool HasChildren() => children.Any();
        public override string ToString() => name;
    }
}
