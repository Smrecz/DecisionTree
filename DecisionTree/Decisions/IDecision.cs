namespace DecisionTree.Decisions
{
    public interface IDecision<in T>
    {
        public void Evaluate(T dto);
    }
}
