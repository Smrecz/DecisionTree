using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DecisionTree.Decisions.DecisionsBase;

namespace DecisionTree.Decisions
{
    public class BinaryDecisionNode<T> : BaseBinaryDecisionNode<T>
    {
        internal BinaryDecisionNode(
            string title, 
            Expression<Func<T, bool>> condition, 
            Dictionary<bool, IDecision<T>> paths) : base(title, condition, paths)
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
                HandleEvaluationException(e);
            }
        }
    }
}
