using DecisionTree.Decisions;

namespace DecisionTree.Builders.Interface
{
    public interface INodeLogic<T, TResult>
    {
        INodeBuild<T, TResult> AddPath(TResult key, IDecision<T> path);
        INodeBuild<T, TResult> AddDefault(IDecision<T> defaultDecision);
    }
}