﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DecisionTree.Exceptions;

namespace DecisionTree.Decisions.DecisionsBase
{
    public abstract class BaseDecisionNode<T, TResult> : BaseNode<T, TResult>
    {
        protected BaseDecisionNode(
            string title,
            Expression<Func<T, TResult>> condition,
            Dictionary<TResult, IDecision<T>> paths,
            IDecision<T> defaultPath = null,
            IDecision<T> nullPath = null,
            Expression<Func<T, T>> action = null) 
            : base(title, condition, paths, defaultPath, nullPath, action)
        {
            _conditionCheck = condition.Compile();
        }

        private readonly Func<T, TResult> _conditionCheck;

        public override void Evaluate(T dto)
        {
            if (TryPathEvaluate(dto))
                return;

            if (TryDefaultEvaluate(dto))
                return;

            var result = _conditionCheck(dto);
            throw new MissingDecisionPathException(result.ToString());
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
            var result = _conditionCheck(dto);

            if (result == null)
            {
                if (NullPath == null)
                    return false;

                NullPath.Evaluate(dto);
                return true;
            }

            if (!Paths.TryGetValue(result, out var decision))
                return false;

            decision.Evaluate(dto);
            return true;
        }
    }
}
