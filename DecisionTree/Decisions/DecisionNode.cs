using DecisionTree.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DecisionTree.Decisions
{
    public class DecisionNode<T, TResult> : IDecision<T>
    {
        public DecisionNode(string title, Expression<Func<T, TResult>> condition, Dictionary<TResult, IDecision<T>> paths, IDecision<T> defaultPath = null, Expression<Func<T, T>> action = null)
        {
            Title = title;
            Condition = condition;
            Paths = paths;
            DefaultPath = defaultPath;
            _conditionCheck = condition.Compile();
            Action = action;
            _actionFunc = action?.Compile();
        }

        public string Title { get; }
        public IDecision<T> DefaultPath { get; }
        public Dictionary<TResult, IDecision<T>> Paths { get; }
        public Expression<Func<T, TResult>> Condition { get; }
        public Expression<Func<T, T>> Action { get; }

        private readonly Func<T, TResult> _conditionCheck;
        private readonly Func<T, T> _actionFunc;

        public void Evaluate(T dto)
        {
            _actionFunc?.Invoke(dto);

            var result = _conditionCheck(dto);

            if (Paths.TryGetValue(result, out var decision))
            {
                decision.Evaluate(dto);
                return;
            }

            if (DefaultPath != null)
                DefaultPath.Evaluate(dto);
            else
                throw new MissingDecisionPathException($"Decision path not defined for result: {result}");
        }
    }
}
