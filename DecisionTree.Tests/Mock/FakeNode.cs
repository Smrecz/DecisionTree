using System;
using DecisionTree.Decisions.DecisionsBase;

namespace DecisionTree.Tests.Mock
{
    public class FakeNode<T> : IDecision<T>
    {
        public void Evaluate(T dto)
        {
            throw new NotImplementedException();
        }
    }
}
