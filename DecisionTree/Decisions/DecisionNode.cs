using DecisionTree.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DecisionTree.Decisions
{
    public class DecisionNode<T, TResult> : IDecision<T>
    {
        internal DecisionNode(
            string title, 
            Expression<Func<T, TResult>> condition, 
            Dictionary<TResult, IDecision<T>> paths, 
            IDecision<T> defaultPath = null, 
            Expression<Func<T, T>> action = null)
        {
            Title = title;
            Condition = condition;
            Paths = paths;
            DefaultPath = defaultPath;
            _conditionCheck = condition.Compile();
            Action = action;
            _actionFunc = action?.Compile();

            if (paths is Dictionary<bool, IDecision<T>> boolPaths)
            {
                boolPaths.TryGetValue(true, out _truePath);
                boolPaths.TryGetValue(false, out _falsePath);
                _isBoolCheck = true;
                _boolConditionCheck = (condition as Expression<Func<T, bool>>).Compile();
            }
        }

        public string Title { get; }
        public IDecision<T> DefaultPath { get; }
        public Dictionary<TResult, IDecision<T>> Paths { get; }
        public Expression<Func<T, TResult>> Condition { get; }
        public Expression<Func<T, T>> Action { get; }

        private readonly IDecision<T> _truePath;
        private readonly IDecision<T> _falsePath;
        private readonly Func<T, bool> _boolConditionCheck;
        private readonly bool _isBoolCheck;
        private readonly Func<T, TResult> _conditionCheck;
        private readonly Func<T, T> _actionFunc;

        public void Evaluate(T dto)
        {
            _actionFunc?.Invoke(dto);

            if (TryBoolEvaluate(dto))
                return;

            if (TryPathEvaluate(dto))
                return;

            if (TryDefaultEvaluate(dto))
                return;

            var result = _conditionCheck(dto);
            throw new MissingDecisionPathException($"Decision path not defined for result: {result}");
        }

        private bool TryDefaultEvaluate(T dto)
        {
            if (DefaultPath == null)
                return false;

            DefaultPath.Evaluate(dto);
            return true;
        }

        private bool TryPathEvaluate(T dto)
        {
            if (!Paths.TryGetValue(_conditionCheck(dto), out var decision))
                return false;

            decision.Evaluate(dto);
            return true;
        }

        private bool TryBoolEvaluate(T dto)
        {
            if (!_isBoolCheck)
                return false;

            var result = _boolConditionCheck(dto);

            if (result && _truePath != null)
            {
                _truePath.Evaluate(dto);
                return true;
            }

            if (!result && _falsePath != null)
            {
                _falsePath.Evaluate(dto);
                return true;
            }

            return false;
        }
    }
}
