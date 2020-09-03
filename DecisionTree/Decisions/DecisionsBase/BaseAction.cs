using System;
using System.Linq.Expressions;

namespace DecisionTree.Decisions.DecisionsBase
{
    public abstract class BaseAction<T> : IDecisionAction<T>
    {
        protected BaseAction(string title, Expression<Func<T, T>> action, IDecision<T> path)
        {
            Action = action;
            Title = title;
            Path = path;
        }

        public string Title { get; private set; }
        public IDecision<T> Path { get; }
        public Expression<Func<T, T>> Action { get; }

        internal void ChangeTitle(string newTitle)
        {
            Title = newTitle;
        }

        public abstract void Evaluate(T dto);
    }
}
