using System;
using System.Linq.Expressions;

namespace DecisionTree.Decisions.DecisionsBase
{
    public abstract class BaseDecisionAction<T> : BaseAction<T>
    {
        protected BaseDecisionAction(string title, 
            Expression<Func<T, T>> action, 
            IDecision<T> path) : base(title, action, path)
        {
            _actionFunc = action.Compile();
        }

        private readonly Func<T, T> _actionFunc;

        public override void Evaluate(T dto)
        {
            _actionFunc.Invoke(dto);
            Path.Evaluate(dto);
        }
    }
}
