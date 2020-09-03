using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DecisionTree.Decisions;
using DecisionTree.Builders.Interface.Action;
using DecisionTree.Builders.Interface.BinaryNode;
using DecisionTree.Decisions.DecisionsBase;

namespace DecisionTree.Builders
{
    public sealed class BinaryDecisionNodeBuilder<T>
        : IBinaryNodeTitle<T>, IBinaryNodeCondition<T>, IBinaryNodeBuild<T>, IBinaryNegativeNodePath<T>, IBinaryPositiveNodePath<T>
    {
        private readonly Dictionary<bool, IDecision<T>> _paths = new Dictionary<bool, IDecision<T>>();
        private Expression<Func<T, bool>> _condition;
        private Expression<Func<T, T>> _action;
        private string _title;

        private BinaryDecisionNodeBuilder() { }

        public static IBinaryNodeTitle<T> Create() => new BinaryDecisionNodeBuilder<T>();

        public IBinaryNodeCondition<T> AddTitle(string title)
        {
            _title = title;
            return this;
        }

        public IBinaryPositiveNodePath<T> AddCondition(Expression<Func<T, bool>> nodeCondition)
        {
            _condition = nodeCondition;
            return this;
        }

        public IBinaryNegativeNodePath<T> AddPositivePath(IDecision<T> path)
        {
            _paths.Add(true, path);
            return this;
        }

        public IBinaryNodeBuild<T> AddNegativePath(IDecision<T> path)
        {
            _paths.Add(false, path);
            return this;
        }

        public IBinaryNegativeNodePath<T> AddPositivePath(IDecision<T> path, IActionPath<T> actionBeforePath)
        {
            var action = actionBeforePath.AddPath(path).Build();

            AddActionPath(true, action);

            return this;
        }

        public IBinaryNodeBuild<T> AddNegativePath(IDecision<T> path, IActionPath<T> actionBeforePath)
        {
            var action = actionBeforePath.AddPath(path).Build();

            AddActionPath(false, action);

            return this;
        }
        public IBinaryNodeBuild<T> AddAction(Expression<Func<T, T>> action)
        {
            _action = action;
            return this;
        }

        public IDecisionNode<T, bool> Build()
        {
            if (_action == null)
                return new BinaryDecisionNode<T>(_title, _condition, _paths);

            return new BinaryDecisionActionNode<T>(_title, _condition, _paths, _action);
        }

        private void AddActionPath(bool key, IDecisionAction<T> action)
        {
            if (action is DecisionAction<T> decisionAction)
                decisionAction.ChangeTitle($"{key} {_title} - {action.Title}");

            _paths[key] = action;
        }
    }
}
