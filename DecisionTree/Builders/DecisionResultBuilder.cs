using DecisionTree.Decisions;
using System;
using System.Linq.Expressions;
using DecisionTree.Builders.Interface;

namespace DecisionTree.Builders
{
    public sealed class DecisionResultBuilder<T> 
        : IResultTitle<T>, IResultBuild<T>
    {
        private string _title;
        private Expression<Func<T, T>> _action;

        private DecisionResultBuilder() { }

        public static IResultTitle<T> Create() => new DecisionResultBuilder<T>();

        public IResultBuild<T> AddTitle(string title)
        {
            _title = title;
            return this;
        }

        public IResultBuild<T> AddAction(Expression<Func<T, T>> action)
        {
            _action = action;
            return this;
        }

        public IDecisionResult<T> Build() => 
            new DecisionResult<T>(_title, _action);
    }
}
