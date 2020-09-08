using System;
using DecisionTree.Decisions.DecisionsBase;

namespace DecisionTree.Decisions
{
    public class DecisionResult<T> : BaseDecisionResult<T>
    {
        internal DecisionResult(string title) : base(title)
        {
        }

        public override void Evaluate(T dto)
        {
            try
            {
                base.Evaluate(dto);
            }
            catch (Exception e)
            {
                HandleEvaluationException(e);
            }
        }
    }
}
