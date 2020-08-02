using DecisionTree.Decisions;
using DecisionTree.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DecisionTree.Builders
{
    public class DecisionNodeBuilder<T, TResult>
    {
        private readonly Dictionary<TResult, IDecision<T>> _paths = new Dictionary<TResult, IDecision<T>>();
        private Expression<Func<T, TResult>> _condition;
        private IDecision<T> _defaultDecision;

        public DecisionNodeBuilder<T, TResult> AddPath(TResult key, IDecision<T> path)
        {
            _paths.Add(key, path);
            return this;
        }

        public DecisionNodeBuilder<T, TResult> AddDefault(IDecision<T> defaultDecision)
        {
            _defaultDecision = defaultDecision;
            return this;
        }

        public DecisionNodeBuilder<T, TResult> AddCondition(Expression<Func<T, TResult>> nodeCondition)
        {
            _condition = nodeCondition;
            return this;
        }

        public DecisionNode<T, TResult> Build()
        {
            if (_condition == null)
                throw new MissingBuilderConfigException($"{nameof(AddCondition)} is mandatory and has to be configured.");
            if (_defaultDecision == null && _paths.Count == 0)
                throw new MissingBuilderConfigException($"Either {nameof(AddDefault)} or at least one {nameof(AddPath)} has to be configured.");

            return new DecisionNode<T, TResult>(_condition, _paths, _defaultDecision);
        }
    }
}
