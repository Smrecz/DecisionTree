namespace DecisionTree.DotTreeExtensions.Dto
{
    public class NodeId
    {
        public int Counter { get; private set; }

        public NodeId() => 
            Counter = 0;

        public void Increment() => 
            Counter++;
    }
}
