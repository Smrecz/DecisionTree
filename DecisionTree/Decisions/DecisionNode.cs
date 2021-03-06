﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DecisionTree.Decisions.DecisionsBase;

namespace DecisionTree.Decisions
{
    public class DecisionNode<T, TResult> : BaseDecisionNode<T, TResult>
    {
        internal DecisionNode(
            string title,
            Expression<Func<T, TResult>> condition,
            Dictionary<TResult, IDecision<T>> paths,
            IDecision<T> defaultPath = null,
            IDecision<T> nullPath = null) 
            : base(title, condition, paths, defaultPath, nullPath)
        {
        }

        public override void Evaluate(T dto)
        {
            try
            {
                base.Evaluate(dto);
            }
            catch (Exception e)
            {
                throw GetEvaluationException(e);
            }
        }
    }
}
