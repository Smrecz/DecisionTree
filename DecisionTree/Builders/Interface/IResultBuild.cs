using System;
using System.Linq.Expressions;
using DecisionTree.Decisions;

namespace DecisionTree.Builders.Interface
{
    public interface IResultBuild<T>
    {
        IResultBuild<T> AddAction(Expression<Func<T, T>> action);
        IDecisionResult<T> Build();
    }
}