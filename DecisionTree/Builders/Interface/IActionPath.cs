using DecisionTree.Decisions;

namespace DecisionTree.Builders.Interface
{
    public interface IActionPath<T>
    {
        IActionBuild<T> AddPath(IDecision<T> path);
    }
}