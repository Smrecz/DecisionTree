using System;
using System.Linq.Expressions;

namespace DecisionTree.Decisions.DecisionsBase
{
    public abstract class BaseAction<T> : DecisionBase, IDecisionAction<T>
    {
        protected BaseAction(string title, Expression<Func<T, T>> action, IDecision<T> path)
            : base(title)
        {
            Action = action;
            Path = path;
        }

        public IDecision<T> Path { get; }
        public Expression<Func<T, T>> Action { get; }

        public abstract void Evaluate(T dto);
    }
}
