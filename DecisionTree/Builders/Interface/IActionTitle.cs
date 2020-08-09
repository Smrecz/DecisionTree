namespace DecisionTree.Builders.Interface
{
    public interface IActionTitle<T>
    {
        IAction<T> AddTitle(string title);
    }
}