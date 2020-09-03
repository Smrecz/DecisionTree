using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DecisionTree.Decisions.DecisionsBase;

namespace DecisionTree.Decisions
{
    public class BinaryDecisionActionNode<T> : BaseBinaryDecisionNode<T>
    {
        internal BinaryDecisionActionNode(
            string title, 
            Expression<Func<T, bool>> condition, 
            Dictionary<bool, IDecision<T>> paths, 
            Expression<Func<T, T>> action) : base(title, condition, paths, action)
        {
            _actionFunc = action.Compile();
        }

        private readonly Func<T, T> _actionFunc;

        public override void Evaluate(T dto)
        {
            _actionFunc.Invoke(dto);

            base.Evaluate(dto);
        }
    }
}
