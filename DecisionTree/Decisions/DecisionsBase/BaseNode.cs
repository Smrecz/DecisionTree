using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DecisionTree.Decisions.DecisionsBase
{
    public abstract class BaseNode<T, TResult> : DecisionBase, IDecisionNode<T, TResult>
    {
        protected BaseNode(
            string title,
            Expression<Func<T, TResult>> condition,
            Dictionary<TResult, IDecision<T>> paths,
            IDecision<T> defaultPath,
            IDecision<T> nullPath,
            Expression<Func<T, T>> action) : base(title)
        {
            Condition = condition;
            Paths = paths;
            DefaultPath = defaultPath;
            NullPath = nullPath;
            Action = action;
        }

        public IDecision<T> NullPath { get; }
        public IDecision<T> DefaultPath { get; }
        public Dictionary<TResult, IDecision<T>> Paths { get; }
        public Expression<Func<T, TResult>> Condition { get; }
        public Expression<Func<T, T>> Action { get; }

        public abstract void Evaluate(T dto);
    }
}
