using BehaviorTree.BT.CompositeNodes;
using BehaviorTree.BT.DecoratorNodes;
using BehaviorTree.BT.Interfaces;
using BehaviorTree.BT.LeafNodes;
using static BehaviorTree.BT.CompositeNodes.SyncParallelNode;
using static BehaviorTree.BT.Node;

/// <summary>
/// BehaviourTreeBuilder class is based on the class with same name on 
/// https://github.com/ashleydavis/Fluent-Behaviour-Tree
/// </summary>

namespace BehaviorTree.BT
{
    public class BehaviourTreeBuilder
    {
        /// <summary>
        /// Last node created.
        /// </summary>
        private INode? currentNode = null;

        /// <summary>
        /// Stack node nodes that we are build via the fluent API.
        /// </summary>
        private readonly Stack<IParentNode> parentNodeStack = new();

        /// <summary>
        /// Create a root node.
        /// </summary>
        public BehaviourTreeBuilder Root(string name, bool executeInLoop)
        {
            if (parentNodeStack.Count > 0)
                throw new InvalidOperationException("Can't add a root node in a tree with nodes");

            var rootNode = new RootNode(name, executeInLoop);

            parentNodeStack.Push(rootNode);
            return this;
        }

        /// <summary>
        /// Create a sequence node.
        /// </summary>
        public BehaviourTreeBuilder Sequence(string name)
        {
            var sequenceNode = new SequenceNode(name);

            if (parentNodeStack.Count > 0)
                parentNodeStack.Peek().Attach(sequenceNode);

            parentNodeStack.Push(sequenceNode);
            return this;
        }

        /// <summary>
        /// Create a sync parallel node.
        /// </summary>
        public BehaviourTreeBuilder SyncParallel(string name, POLICY policySuccess, POLICY policyFailure, STATUS statusWhenInInfinteLoop)
        {
            var parallelNode = new SyncParallelNode(name, policySuccess, policyFailure, statusWhenInInfinteLoop);

            if (parentNodeStack.Count > 0)
                parentNodeStack.Peek().Attach(parallelNode);

            parentNodeStack.Push(parallelNode);
            return this;
        }

        /// <summary>
        /// Create a selector(fallback) node.
        /// </summary>
        public BehaviourTreeBuilder Selector(string name)
        {
            var selectorNode = new SelectorNode(name);

            if (parentNodeStack.Count > 0)
                parentNodeStack.Peek().Attach(selectorNode);

            parentNodeStack.Push(selectorNode);
            return this;
        }

        /// <summary>
        /// Create an inverter node that inverts the success/failure of it child.
        /// </summary>
        public BehaviourTreeBuilder Inverter(string name)
        {
            var inverterNode = new InverterNode(name);

            if (parentNodeStack.Count > 0)
                parentNodeStack.Peek().Attach(inverterNode);

            parentNodeStack.Push(inverterNode);
            return this;
        }

        /// <summary>
        /// Create an force success node that returns always SUCCESS case success/failure of it child.
        /// </summary>
        public BehaviourTreeBuilder ForceSuccess(string name)
        {
            var forceSuccessNode = new ForceSuccessNode(name);

            if (parentNodeStack.Count > 0)
                parentNodeStack.Peek().Attach(forceSuccessNode);

            parentNodeStack.Push(forceSuccessNode);
            return this;
        }

        /// <summary>
        /// Create an force failure node that returns always FAILURE case success/failure of it child.
        /// </summary>
        public BehaviourTreeBuilder ForceFailure(string name)
        {
            var forceFailureNode = new ForceFailureNode(name);

            if (parentNodeStack.Count > 0)
                parentNodeStack.Peek().Attach(forceFailureNode);

            parentNodeStack.Push(forceFailureNode);
            return this;
        }

        /// <summary>
        /// Create an retry node that tick the child up to N times as long as the child returns FAILURE.
        /// Interrupt the loop if the child returns SUCCESS and, in that case, return SUCCESS too.
        /// </summary>
        public BehaviourTreeBuilder Retry(string name, int times)
        {
            var retryNode = new RetryNode(name, times);

            if (parentNodeStack.Count > 0)
                parentNodeStack.Peek().Attach(retryNode);

            parentNodeStack.Push(retryNode);
            return this;
        }

        /// <summary>
        /// Create an repeat node that tick the child up to N times as long as the child returns SUCCESS.
        /// Interrupt the loop if the child returns FAILURE and, in that case, return FAILURE too.
        /// </summary>
        public BehaviourTreeBuilder Repeat(string name, int times)
        {
            var repeatNode = new RepeatNode(name, times);

            if (parentNodeStack.Count > 0)
                parentNodeStack.Peek().Attach(repeatNode);

            parentNodeStack.Push(repeatNode);
            return this;
        }

        /// <summary>
        /// Create an action node.
        /// </summary>
        public BehaviourTreeBuilder Action(string name, ActionNode.ProcessNode action)
        {
            if (parentNodeStack.Count <= 0)
                throw new InvalidOperationException("Can't create an unnested Action Node, it must be a leaf node.");

            var actionNode = new ActionNode(name, action);
            parentNodeStack.Peek().Attach(actionNode);
            return this;
        }

        /// <summary>
        /// Create an condition node.
        /// </summary>
        public BehaviourTreeBuilder Condition(string name, ConditionNode.ProcessNode action)
        {
            if (parentNodeStack.Count <= 0)
                throw new InvalidOperationException("Can't create an unnested Condition Node, it must be a leaf node.");

            var actionNode = new ConditionNode(name, action);
            parentNodeStack.Peek().Attach(actionNode);
            return this;
        }

        /// <summary>
        /// Build the actual tree.
        /// </summary>
        public INode Build()
        {
            if (currentNode == null)
                throw new InvalidOperationException("Can't create a behaviour tree with zero nodes");
            
            return currentNode;
        }

        /// <summary>
        /// Ends a sequence of children.
        /// </summary>
        public BehaviourTreeBuilder End()
        {
            currentNode = parentNodeStack.Pop();
            return this;
        }
    }
}