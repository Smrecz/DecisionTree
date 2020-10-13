namespace DecisionTree.DotTreeExtensions
{
    public class GraphOptions
    {
        /// <summary>
        /// Will generate graph definition with horizontal layout attribute.
        /// </summary>
        public bool IsHorizontal { get; set; }
        /// <summary>
        /// Will generate graph definition where each node visit has unique Id and separate block.
        /// </summary>
        public bool UseUniquePaths { get; set; }
        /// <summary>
        /// Will generate graph definition with only titles and paths.
        /// </summary>
        public bool TitleOnly { get; set; }
        /// <summary>
        /// Custom graph style configuration. Will override default styling where provided.
        /// </summary>
        public GraphStyle GraphStyle { get; set; }
    }
}
