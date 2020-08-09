using System;
using System.Linq.Expressions;

namespace DecisionTree.Decisions
{
    public class DecisionResult<T> : IDecision<T>
    {
        internal DecisionResult(string title, Expression<Func<T, T>> action = null)
        {
            Action = action;
            _actionFunc = action?.Compile();
            Title = title;
        }

        public string Title { get; }
        public Expression<Func<T, T>> Action { get; }

        private readonly Func<T, T> _actionFunc;

        public void Evaluate(T dto) => _actionFunc?.Invoke(dto);
    }
}
