using System;
using System.Linq.Expressions;

namespace DecisionTree.Decisions
{
    public interface IDecisionResult<T> : IDecision<T>
    {
        string Title { get; }
        Expression<Func<T, T>> Action { get; }
    }
}