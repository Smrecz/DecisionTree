using DecisionTree.Decisions;
using System;
using System.Linq.Expressions;
using DecisionTree.Builders.Interface;

namespace DecisionTree.Builders
{
    public sealed class DecisionActionBuilder<T> 
        : IActionTitle<T>, IAction<T>, IActionPath<T>, IActionBuild<T>
    {
        private string _title;
        private Expression<Func<T, T>> _action;
        private IDecision<T> _path;

        private DecisionActionBuilder() { }

        public static IActionTitle<T> Create() => new DecisionActionBuilder<T>();

        public IAction<T> AddTitle(string title)
        {
            _title = title;
            return this;
        }

        public IActionPath<T> AddAction(Expression<Func<T, T>> action)
        {
            _action = action;
            return this;
        }

        public IActionBuild<T> AddPath(IDecision<T> path)
        {
            _path = path;
            return this;
        }

        public DecisionAction<T> Build() => 
            new DecisionAction<T>(_title, _action, _path);
    }
}
