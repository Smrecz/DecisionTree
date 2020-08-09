using DecisionTree.Decisions;

namespace DecisionTree.Builders.Interface
{
    public interface IActionBuild<T>
    {
        DecisionAction<T> Build();
    }
}