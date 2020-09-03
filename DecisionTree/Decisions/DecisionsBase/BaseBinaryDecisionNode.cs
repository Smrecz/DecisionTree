using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DecisionTree.Decisions.DecisionsBase
{
    public abstract class BaseBinaryDecisionNode<T> : BaseNode<T, bool>
    {
        protected BaseBinaryDecisionNode(
            string title,
            Expression<Func<T, bool>> condition,
            Dictionary<bool, IDecision<T>> paths,
            Expression<Func<T, T>> action = null) : base(title, condition, paths, null, action)
        {
            _conditionCheck = condition.Compile();

            paths.TryGetValue(true, out _truePath);
            paths.TryGetValue(false, out _falsePath);
        }

        private readonly IDecision<T> _truePath;
        private readonly IDecision<T> _falsePath;
        private readonly Func<T, bool> _conditionCheck;

        public override void Evaluate(T dto)
        {
            var result = _conditionCheck(dto);

            if (result)
                _truePath.Evaluate(dto);

            if (!result)
                _falsePath.Evaluate(dto);
        }
    }
}
