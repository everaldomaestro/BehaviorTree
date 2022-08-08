namespace BehaviorTree.Abstract
{
    public abstract class RandomComposite : Composite
    {
        protected RandomComposite(string name) : base(name) { }

        protected virtual bool ChildTickRandom()
        {
            var idleChildren = children.Where(x => x.State == STATE.IDLE).ToList();
            if (!idleChildren.Any())
                return false;

            var randomIndex = new Random().Next(0, idleChildren.Count - 1);
            currentChild = children.IndexOf(idleChildren[randomIndex]);

            return true;
        }
    }
}
