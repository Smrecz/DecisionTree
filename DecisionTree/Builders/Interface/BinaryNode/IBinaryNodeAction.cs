using System;
using System.Linq.Expressions;

namespace DecisionTree.Builders.Interface.BinaryNode
{
    public interface IBinaryNodeAction<T>
    {
        IBinaryNodeBuild<T> AddAction(Expression<Func<T, T>> action);
    }
}