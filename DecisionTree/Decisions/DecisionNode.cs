using DecisionTree.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DecisionTree.Decisions
{
    public class DecisionNode<T, TResult> : IDecision<T>
    {
        public DecisionNode(Expression<Func<T, TResult>> condition, Dictionary<TResult, IDecision<T>> paths, IDecision<T> defaultPath = null)
        {
            Condition = condition;
            Paths = paths;
            DefaultPath = defaultPath;
            _conditionCheck = condition.Compile();
        }

        public IDecision<T> DefaultPath { get; }
        public Dictionary<TResult, IDecision<T>> Paths { get; }
        public Expression<Func<T, TResult>> Condition { get; }

        private readonly Func<T, TResult> _conditionCheck;

        public void Evaluate(T dto)
        {
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
