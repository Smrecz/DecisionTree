﻿using System;
using System.Linq.Expressions;
using DecisionTree.Decisions.DecisionsBase;
using DecisionTree.Exceptions;

namespace DecisionTree.Decisions
{
    public class DecisionAction<T> : BaseDecisionAction<T>
    {
        internal DecisionAction(string title, 
            Expression<Func<T, T>> action, 
            IDecision<T> path) : base(title, action, path)
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
