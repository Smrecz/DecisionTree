using System;
using System.Linq.Expressions;

namespace DecisionTree.Builders.Interface
{
    public interface IAction<T>
    {
        IActionPath<T> AddAction(Expression<Func<T, T>> action);
    }
}