using DecisionTree.Decisions;

namespace DecisionTree.Tree
{
    public abstract class DecisionTreeBase<T>
    {
        public abstract IDecision<T> GetTrunk();
    }
}
