using System;
using System.Linq.Expressions;

namespace DecisionTree.Decisions.DecisionsBase
{
    public abstract class BaseResult<T> : IDecisionResult<T>
    {
        protected BaseResult(string title, Expression<Func<T, T>> action = null)
        {
            Action = action;
            Title = title;
        }

        public string Title { get; }
        public Expression<Func<T, T>> Action { get; }

        public abstract void Evaluate(T dto);
    }
}
