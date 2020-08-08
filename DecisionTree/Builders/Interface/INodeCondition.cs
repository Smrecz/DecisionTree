using System;
using System.Linq.Expressions;

namespace DecisionTree.Builders.Interface
{
    public interface INodeCondition<T, TResult>
    {
        INodeLogic<T, TResult> AddCondition(Expression<Func<T, TResult>> nodeCondition);
    }
}