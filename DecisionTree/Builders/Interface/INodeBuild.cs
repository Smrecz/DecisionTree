using DecisionTree.Decisions;

namespace DecisionTree.Builders.Interface
{
    public interface INodeBuild<T, TResult> 
        : INodeLogic<T, TResult>, INodeAction<T, TResult>
    {
        DecisionNode<T, TResult> Build();
    }
}