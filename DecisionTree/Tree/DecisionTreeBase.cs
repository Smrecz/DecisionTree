using DecisionTree.Decisions;

namespace DecisionTree.Tree
{
    public abstract class DecisionTreeBase<T, TResult>
    {
        public abstract DecisionNode<T, TResult> Trunk();
    }
}
