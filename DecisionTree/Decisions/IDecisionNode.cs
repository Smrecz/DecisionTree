using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DecisionTree.Decisions
{
    public interface IDecisionNode<T, TResult> : IDecision<T>
    {
        string Title { get; }
        IDecision<T> DefaultPath { get; }
        Dictionary<TResult, IDecision<T>> Paths { get; }
        Expression<Func<T, TResult>> Condition { get; }
        Expression<Func<T, T>> Action { get; }
    }
}