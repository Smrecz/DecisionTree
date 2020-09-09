﻿using System;
using System.Linq.Expressions;

namespace DecisionTree.Decisions.DecisionsBase
{
    public interface IDecisionAction<T> : IDecision<T>
    {
        string Title { get; }
        IDecision<T> Path { get; }
        Expression<Func<T, T>> Action { get; }
    }
}