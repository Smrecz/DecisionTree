using System;
using System.Linq.Expressions;

namespace DecisionTree.Decisions.DecisionsBase
{
    public interface IDecisionResult<T> : IDecision<T>
    {
        string Title { get; }
        Expression<Func<T, T>> Action { get; }
    }
}