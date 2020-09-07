using System;
using System.Linq.Expressions;
using DecisionTree.Decisions.DecisionsBase;
using DecisionTree.Exceptions;

namespace DecisionTree.Decisions
{
    public class DecisionActionResult<T> : BaseDecisionResult<T>
    {
        internal DecisionActionResult(string title,
            Expression<Func<T, T>> action) : base(title, action)
        {
            _actionFunc = action.Compile();
        }


        private readonly Func<T, T> _actionFunc;

        public override void Evaluate(T dto)
        {
            try
            {
                _actionFunc.Invoke(dto);

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
