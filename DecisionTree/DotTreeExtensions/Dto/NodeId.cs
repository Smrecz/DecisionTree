namespace DecisionTree.DotTreeExtensions.Dto
{
    internal class NodeId
    {
        public int Counter { get; private set; }

        public NodeId() => 
            Counter = 0;

        public void Increment() => 
            Counter++;
    }
}
