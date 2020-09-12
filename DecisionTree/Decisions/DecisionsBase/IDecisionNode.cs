using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DecisionTree.Decisions.DecisionsBase
{
    public interface IDecisionNode<T, TResult> : IDecision<T>, ITitled
    {
        IDecision<T> DefaultPath { get; }
        IDecision<T> NullPath { get; }
        Dictionary<TResult, IDecision<T>> Paths { get; }
        Expression<Func<T, TResult>> Condition { get; }
        Expression<Func<T, T>> Action { get; }
    }
}