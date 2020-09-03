using DecisionTree.Builders.Interface.Action;
using DecisionTree.Decisions.DecisionsBase;

namespace DecisionTree.Builders.Interface.Node
{
    public interface INodePath<T, TResult>
    {
        INodeBuild<T, TResult> AddPath(TResult key, IDecision<T> path);
        INodeBuild<T, TResult> AddPath(TResult key, IDecision<T> path, IActionPath<T> pathAction);
    }
}