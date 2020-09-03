using DecisionTree.Builders.Interface.Action;
using DecisionTree.Decisions.DecisionsBase;

namespace DecisionTree.Builders.Interface.BinaryNode
{
    public interface IBinaryPositiveNodePath<T>
    {
        IBinaryNegativeNodePath<T> AddPositivePath(IDecision<T> path);
        IBinaryNegativeNodePath<T> AddPositivePath(IDecision<T> path, IActionPath<T> pathAction);
    }
}