namespace DecisionTree.Builders.Interface
{
    public interface INodeTitle<T, TResult>
    {
        INodeCondition<T, TResult> AddTitle(string title);
    }
}