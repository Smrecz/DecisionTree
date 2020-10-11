namespace DecisionTree.DotTreeExtensions.Dto
{
    internal class GraphConfig
    {
        public GraphConfig(NodeId nodeId, bool titleOnly)
        {
            NodeId = nodeId;
            TitleOnly = titleOnly;
        }

        public NodeId NodeId { get; }
        public bool TitleOnly { get; }
    }
}
