using DecisionTree.Decisions.DecisionsBase;

namespace DecisionTree.Builders.Interface.Action
{
    public interface IActionBuild<T>
    {
        IDecisionAction<T> Build();
    }
}