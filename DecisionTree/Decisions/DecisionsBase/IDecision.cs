namespace DecisionTree.Decisions.DecisionsBase
{
    public interface IDecision<in T>
    {
        public void Evaluate(T dto);
    }
}
