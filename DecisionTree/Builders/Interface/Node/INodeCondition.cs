using System;
using System.Linq.Expressions;

namespace DecisionTree.Builders.Interface.Node
{
    public interface INodeCondition<T, TResult>
    {
        INodePath<T, TResult> AddCondition(Expression<Func<T, TResult>> nodeCondition);
    }
}