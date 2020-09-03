namespace DecisionTree.Builders.Interface.Action
{
    public interface IActionTitle<T>
    {
        IAction<T> AddTitle(string title);
    }
}