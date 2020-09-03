namespace DecisionTree.Builders.Interface.Result
{
    public interface IResultTitle<T>
    {
        IResultBuild<T> AddTitle(string title);
    }
}