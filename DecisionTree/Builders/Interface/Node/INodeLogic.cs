using DecisionTree.Decisions.DecisionsBase;

namespace DecisionTree.Builders.Interface.Node
{
    public interface INodeLogic<T, TResult> : INodePath<T, TResult>
    {
        INodeBuild<T, TResult> AddDefault(IDecision<T> defaultDecision);
    }
}