using DecisionTree.Decisions.DecisionsBase;

namespace DecisionTree.Builders.Interface.Action
{
    public interface IActionPath<T>
    {
        IActionBuild<T> AddPath(IDecision<T> path);
    }
}