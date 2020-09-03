using System;
using System.Linq.Expressions;
using DecisionTree.Decisions.DecisionsBase;

namespace DecisionTree.Builders.Interface.Result
{
    public interface IResultBuild<T>
    {
        IResultBuild<T> AddAction(Expression<Func<T, T>> action);
        IDecisionResult<T> Build();
    }
}