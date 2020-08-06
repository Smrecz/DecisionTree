using DecisionTree.Decisions;
using DecisionTree.Exceptions;
using System;
using System.Linq.Expressions;

namespace DecisionTree.Builders
{
    public class DecisionResultBuilder<T>
    {
        private string _title;
        private Expression<Func<T, T>> _action;

        public DecisionResultBuilder<T> AddTitle(string title)
        {
            _title = title;
            return this;
        }

        public DecisionResultBuilder<T> AddAction(Expression<Func<T, T>> action)
        {
            _action = action;
            return this;
        }

        public DecisionResult<T> Build()
        {
            if (_title == null)
                throw new MissingBuilderConfigException($"{nameof(_title)} has to be configured.");
            return new DecisionResult<T>(_title, _action);
        }
    }
}
