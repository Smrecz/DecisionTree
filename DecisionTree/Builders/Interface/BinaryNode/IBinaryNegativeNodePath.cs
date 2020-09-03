using DecisionTree.Builders.Interface.Action;
using DecisionTree.Decisions.DecisionsBase;

namespace DecisionTree.Builders.Interface.BinaryNode
{
    public interface IBinaryNegativeNodePath<T>
    {
        IBinaryNodeBuild<T> AddNegativePath(IDecision<T> path);
        IBinaryNodeBuild<T> AddNegativePath(IDecision<T> path, IActionPath<T> pathAction);
    }
}