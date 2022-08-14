using BehaviorTree.Abstract.Interfaces;

namespace BehaviorTree.Abstract
{
    public abstract class Decorator : Node, IParentNode
    {
        protected Decorator(string name) : base(name) { type = TYPE.DECORATOR; }

        public virtual void Attach(Node child)
        {
            if (HasChildren())
                throw new InvalidOperationException($"Unable to add Node {child}. Only one child is allowed on the decorator node.");

            SetParent(child);
            children.Add(child);
        }

        public virtual void Detach(Node child)
        {
            SetParent(child, true);
            children.Remove(child);
        }
    }
}
