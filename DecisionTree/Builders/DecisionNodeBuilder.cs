using DecisionTree.Decisions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DecisionTree.Builders.Interface;

namespace DecisionTree.Builders
{
    public sealed class DecisionNodeBuilder<T, TResult> 
        : INodeTitle<T, TResult>, INodeCondition<T, TResult>, INodeBuild<T, TResult>
    {
        private readonly Dictionary<TResult, IDecision<T>> _paths = new Dictionary<TResult, IDecision<T>>();
        private Expression<Func<T, TResult>> _condition;
        private IDecision<T> _defaultDecision;
        private Expression<Func<T, T>> _action;
        private string _title;

        private DecisionNodeBuilder() { }

        public static INodeTitle<T, TResult> Create() => new DecisionNodeBuilder<T, TResult>();

        public INodeCondition<T, TResult> AddTitle(string title)
        {
            _title = title;
            return this;
        }

        public INodeLogic<T, TResult> AddCondition(Expression<Func<T, TResult>> nodeCondition)
        {
            _condition = nodeCondition;
            return this;
        }

        public INodeBuild<T, TResult> AddAction(Expression<Func<T, T>> action)
        {
            _action = action;
            return this;
        }

        public INodeBuild<T, TResult> AddPath(TResult key, IDecision<T> path)
        {
            _paths.Add(key, path);
            return this;
        }

        public INodeBuild<T, TResult> AddDefault(IDecision<T> defaultDecision)
        {
            _defaultDecision = defaultDecision;
            return this;
        }

        public DecisionNode<T, TResult> Build() => 
            new DecisionNode<T, TResult>(_title, _condition, _paths, _defaultDecision, _action);
    }
}
