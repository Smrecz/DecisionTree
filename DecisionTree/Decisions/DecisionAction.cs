﻿using System;
using System.Linq.Expressions;

namespace DecisionTree.Decisions
{
    public class DecisionAction<T> : IDecision<T>
    {
        internal DecisionAction(string title, Expression<Func<T, T>> action, IDecision<T> path)
        {
            Action = action;
            _actionFunc = action?.Compile();
            Title = title;
            Path = path;
        }
        public string Title { get; }
        public IDecision<T> Path { get; }
        public Expression<Func<T, T>> Action { get; }

        private readonly Func<T, T> _actionFunc;

        public void Evaluate(T dto)
        {
            _actionFunc.Invoke(dto);
            Path.Evaluate(dto);
        }
    }
}
