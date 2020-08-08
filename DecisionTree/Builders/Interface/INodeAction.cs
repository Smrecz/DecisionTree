using System;
using System.Linq.Expressions;

namespace DecisionTree.Builders.Interface
{
    public interface INodeAction<T, TResult>
    {
        INodeBuild<T, TResult> AddAction(Expression<Func<T, T>> action);
    }
}