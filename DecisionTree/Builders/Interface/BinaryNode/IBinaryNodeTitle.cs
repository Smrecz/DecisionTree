namespace DecisionTree.Builders.Interface.BinaryNode
{
    public interface IBinaryNodeTitle<T>
    {
        IBinaryNodeCondition<T> AddTitle(string title);
    }
}