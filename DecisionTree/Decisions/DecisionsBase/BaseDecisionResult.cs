using System;
using System.Linq.Expressions;

namespace DecisionTree.Decisions.DecisionsBase
{
    public abstract class BaseDecisionResult<T> : BaseResult<T>
    {
        protected BaseDecisionResult(string title,
            Expression<Func<T, T>> action = null) 
            : base (title, action)
        {
        }
        public override void Evaluate(T dto)
        {
        }
    }
}
