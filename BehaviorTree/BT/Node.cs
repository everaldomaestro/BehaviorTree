namespace BehaviorTree.BT
{
    public class Node
    {
        public Node(string name)
        {
            this.name = name;
        }

        public enum STATUS { RUNNING, SUCCESS, FAILURE }
        public enum TYPE { DECORATOR, CONTROL, CONDITION, ACTION }

        protected STATUS status;
        protected TYPE type;
        protected int currentChild = 0;
        protected string name;

        protected List<Node> childrens = new List<Node>();

        public virtual STATUS Process()
        {
            return STATUS.RUNNING;
        }

        public virtual void AddChild(Node child)
        {
            childrens.Add(child);
        }

        public STATUS GetStatus() => status;

        public override string ToString()
        {
            return name;
        }
    }
}
