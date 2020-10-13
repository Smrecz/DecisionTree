namespace DecisionTree.DotTreeExtensions.Dto
{
    internal class GraphConfig
    {
        public GraphConfig(GraphOptions options)
        {
            NodeId = options.UseUniquePaths ? new NodeId() : null;
            TitleOnly = options.TitleOnly;
            Style = options.GraphStyle ?? new GraphStyle();
        }

        public NodeId NodeId { get; }
        public bool TitleOnly { get; }
        public GraphStyle Style { get; }
    }
}
