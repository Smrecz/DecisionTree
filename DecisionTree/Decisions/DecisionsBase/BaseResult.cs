using System;
using System.Linq.Expressions;

namespace DecisionTree.Decisions.DecisionsBase
{
    public abstract class BaseResult<T> : DecisionBase, IDecisionResult<T>
    {
        protected BaseResult(string title, Expression<Func<T, T>> action = null)
            : base(title)
        {
            Action = action;
        }

        public Expression<Func<T, T>> Action { get; }

        public abstract void Evaluate(T dto);
    }
}
