using DecisionTree.Decisions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DecisionTree.Builders.Interface.Action;
using DecisionTree.Builders.Interface.Node;
using DecisionTree.Decisions.DecisionsBase;

namespace DecisionTree.Builders
{
    public sealed class DecisionNodeBuilder<T, TResult>
        : INodeTitle<T, TResult>, INodeCondition<T, TResult>, INodeBuild<T, TResult>
    {
        private readonly Dictionary<TResult, IDecision<T>> _paths = new Dictionary<TResult, IDecision<T>>();
        private Expression<Func<T, TResult>> _condition;
        private IDecision<T> _defaultDecision;
        private IDecision<T> _nullDecision;
        private Expression<Func<T, T>> _action;
        private string _title;

        private DecisionNodeBuilder() { }

        public static INodeTitle<T, TResult> Create() => new DecisionNodeBuilder<T, TResult>();

        private const string NullActionPathText = "#null";

        public INodeCondition<T, TResult> AddTitle(string title)
        {
            _title = title;
            return this;
        }

        public INodePath<T, TResult> AddCondition(Expression<Func<T, TResult>> nodeCondition)
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
            if (TryAddNullPath(key, path))
                return this;

            _paths.Add(key, path);
            return this;
        }

        public INodeBuild<T, TResult> AddPath(TResult key, IDecision<T> path, IActionPath<T> actionBeforePath)
        {
            var action = actionBeforePath.AddPath(path).Build();

            if (action is DecisionAction<T> decisionAction)
                decisionAction.ChangeTitle($"{GetKeyText(key)} {_title} - {action.Title}");

            if (TryAddNullPath(key, action))
                return this;

            _paths.Add(key, action);

            return this;
        }

        private static string GetKeyText(TResult key) => 
            key == null 
                ? NullActionPathText 
                : key.ToString();

        private bool TryAddNullPath(TResult key, IDecision<T> path)
        {
            if (key != null)
                return false;

            _nullDecision = path;
            return true;
        }

        public INodeBuild<T, TResult> AddDefault(IDecision<T> defaultDecision)
        {
            _defaultDecision = defaultDecision;
            return this;
        }

        public IDecisionNode<T, TResult> Build()
        {
            if (_action == null)
                return new DecisionNode<T, TResult>(_title, _condition, _paths, _defaultDecision, _nullDecision);

            return new DecisionActionNode<T, TResult>(_title, _condition, _paths, _action, _defaultDecision, _nullDecision);
        }
    }
}
