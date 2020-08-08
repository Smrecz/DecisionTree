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
    }
}
