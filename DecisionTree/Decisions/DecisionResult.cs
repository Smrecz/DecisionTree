using DecisionTree.Decisions.DecisionsBase;

namespace DecisionTree.Decisions
{
    public class DecisionResult<T> : BaseDecisionResult<T>
    {
        internal DecisionResult(string title) : base(title)
        {
        }
    }
}
