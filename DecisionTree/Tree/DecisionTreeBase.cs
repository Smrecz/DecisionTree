using DecisionTree.Builders;
using DecisionTree.Builders.Interface.Action;
using DecisionTree.Builders.Interface.BinaryNode;
using DecisionTree.Builders.Interface.Node;
using DecisionTree.Builders.Interface.Result;
using DecisionTree.Decisions.DecisionsBase;

namespace DecisionTree.Tree
{
    public abstract class DecisionTreeBase<T>
    {
        public abstract IDecision<T> GetTrunk();

        protected static IBinaryNodeTitle<T> GetBinaryNodeBuilder() =>
            BinaryDecisionNodeBuilder<T>.Create();

        protected static INodeTitle<T, TResult> GetNodeBuilder<TResult>() =>
            DecisionNodeBuilder<T, TResult>.Create();

        protected static IResultTitle<T> GetResultBuilder() =>
            DecisionResultBuilder<T>.Create();

        protected static IActionTitle<T> GetActionBuilder() =>
            DecisionActionBuilder<T>.Create();
    }
}
