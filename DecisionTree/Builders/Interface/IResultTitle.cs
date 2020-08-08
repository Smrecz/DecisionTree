namespace DecisionTree.Builders.Interface
{
    public interface IResultTitle<T>
    {
        IResultBuild<T> AddTitle(string title);
    }
}