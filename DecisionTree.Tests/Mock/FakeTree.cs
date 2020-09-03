using DecisionTree.Decisions.DecisionsBase;
using DecisionTree.Tree;

namespace DecisionTree.Tests.Mock
{
    public class FakeTree<T> : DecisionTreeBase<T>
    {
        public override IDecision<T> GetTrunk() =>
            new FakeNode<T>();
    }
}
