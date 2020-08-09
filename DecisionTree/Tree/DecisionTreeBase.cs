using DecisionTree.Builders;
using DecisionTree.Builders.Interface;
using DecisionTree.Decisions;

namespace DecisionTree.Tree
{
    public abstract class DecisionTreeBase<T>
    {
        public abstract IDecision<T> GetTrunk();

        protected static INodeTitle<T, TResult> GetNodeBuilder<TResult>() =>
            DecisionNodeBuilder<T, TResult>.Create();

        protected static IResultTitle<T> GetResultBuilder() =>
            DecisionResultBuilder<T>.Create();

        protected static IActionTitle<T> GetActionBuilder() =>
            DecisionActionBuilder<T>.Create();
    }
}
