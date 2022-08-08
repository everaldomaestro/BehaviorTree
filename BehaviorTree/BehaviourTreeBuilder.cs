using BehaviorTree.CompositeNodes;
using BehaviorTree.DecoratorNodes;
using BehaviorTree.Abstract.Interfaces;
using BehaviorTree.LeafNodes;
using static BehaviorTree.CompositeNodes.SyncParallelNode;
using static BehaviorTree.Abstract.Node;

/// <summary>
/// BehaviourTreeBuilder class is based on the class with same name on 
/// https://github.com/ashleydavis/Fluent-Behaviour-Tree
/// </summary>

namespace BehaviorTree
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
        public BehaviourTreeBuilder Root(string name, bool executeInLoop = true)
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
        /// Create a random sequence node that tick a random child.
        /// </summary>
        public BehaviourTreeBuilder RandomSequence(string name)
        {
            var randomSequenceNode = new RandomSequenceNode(name);

            if (parentNodeStack.Count > 0)
                parentNodeStack.Peek().Attach(randomSequenceNode);

            parentNodeStack.Push(randomSequenceNode);
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
        public BehaviourTreeBuilder Fallback(string name)
        {
            var fallbackNode = new FallbackNode(name);

            if (parentNodeStack.Count > 0)
                parentNodeStack.Peek().Attach(fallbackNode);

            parentNodeStack.Push(fallbackNode);
            return this;
        }

        /// <summary>
        /// Create a random selector(fallback) node.
        /// </summary>
        public BehaviourTreeBuilder RandomFallback(string name)
        {
            var randomFallbackNode = new RandomFallbackNode(name);

            if (parentNodeStack.Count > 0)
                parentNodeStack.Peek().Attach(randomFallbackNode);

            parentNodeStack.Push(randomFallbackNode);
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