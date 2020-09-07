﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DecisionTree.Decisions.DecisionsBase;
using DecisionTree.Exceptions;

namespace DecisionTree.Decisions
{
    public class DecisionNode<T, TResult> : BaseDecisionNode<T, TResult>
    {
        internal DecisionNode(
            string title,
            Expression<Func<T, TResult>> condition,
            Dictionary<TResult, IDecision<T>> paths,
            IDecision<T> defaultPath = null) : base(title, condition, paths, defaultPath)
        {
        }

        public override void Evaluate(T dto)
        {
            try
            {
                base.Evaluate(dto);
            }
            catch (DecisionEvaluationException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new DecisionEvaluationException(DecisionExceptionMessage, e);
            }
        }
    }
}
