using DecisionTree.Decisions.DecisionsBase;

namespace DecisionTree.Builders.Interface.BinaryNode
{
    public interface IBinaryNodeBuild<T> 
        : IBinaryNodeAction<T>
    {
        IDecisionNode<T, bool> Build();
    }
}