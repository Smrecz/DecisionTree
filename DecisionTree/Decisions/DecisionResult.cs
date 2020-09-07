using System;
using DecisionTree.Decisions.DecisionsBase;
using DecisionTree.Exceptions;

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
            catch (DecisionEvaluationException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new DecisionEvaluationException(DecisionExceptionMessage, e);
            }
        }
    }
}
