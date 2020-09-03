using System;
using System.Linq.Expressions;

namespace DecisionTree.Builders.Interface.BinaryNode
{
    public interface IBinaryNodeCondition<T>
    {
        IBinaryPositiveNodePath<T> AddCondition(Expression<Func<T, bool>> nodeCondition);
    }
}