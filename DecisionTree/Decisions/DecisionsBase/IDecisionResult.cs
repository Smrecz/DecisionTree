using System;
using System.Linq.Expressions;

namespace DecisionTree.Decisions.DecisionsBase
{
    public interface IDecisionResult<T> : IDecision<T>, ITitled
    {
        Expression<Func<T, T>> Action { get; }
    }
}