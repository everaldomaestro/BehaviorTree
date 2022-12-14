using BehaviorTree.Abstract.Interfaces;

namespace BehaviorTree.Abstract
{
    public abstract class Composite : Node, IParentNode
    {
        protected Composite(string name) : base(name) { type = TYPE.COMPOSITE; }

        public virtual void Attach(Node child)
        {
            SetParent(child);
            children.Add(child);
        }

        public virtual void Detach(Node child)
        {
            SetParent(child, true);
            children.Remove(child);
        }

        protected bool IsTheLastChild() => currentChild == children.Count - 1;
    }
}
