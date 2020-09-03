using DecisionTree.Decisions.DecisionsBase;

namespace DecisionTree.Builders.Interface.Node
{
    public interface INodeBuild<T, TResult> 
        : INodeLogic<T, TResult>, INodeAction<T, TResult>
    {
        IDecisionNode<T, TResult> Build();
    }
}