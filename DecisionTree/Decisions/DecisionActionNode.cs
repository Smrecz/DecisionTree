using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DecisionTree.Decisions.DecisionsBase;

namespace DecisionTree.Decisions
{
    public class DecisionActionNode<T, TResult> : BaseDecisionNode<T, TResult>
    {
        internal DecisionActionNode(
            string title,
            Expression<Func<T, TResult>> condition,
            Dictionary<TResult, IDecision<T>> paths,
            Expression<Func<T, T>> action,
            IDecision<T> defaultPath = null) : base(title, condition, paths, defaultPath, action)
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
