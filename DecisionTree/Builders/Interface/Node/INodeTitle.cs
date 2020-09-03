namespace DecisionTree.Builders.Interface.Node
{
    public interface INodeTitle<T, TResult>
    {
        INodeCondition<T, TResult> AddTitle(string title);
    }
}