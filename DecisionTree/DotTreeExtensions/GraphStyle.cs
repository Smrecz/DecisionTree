namespace DecisionTree.DotTreeExtensions
{
    public class GraphStyle
    {
        /// <summary>
        /// Block color for Decisions. Use HTML like Hex Color Code "#RRGGBB" or color name "black".
        /// </summary>
        public string DecisionColor { get; set; } = "#17a2b8";

        /// <summary>
        /// Block color for Decisions with Action. Use HTML like Hex Color Code "#RRGGBB" or color name "black".
        /// </summary>
        public string DecisionActionColor { get; set; } = "#007bff";

        /// <summary>
        /// Block color for Results. Use HTML like Hex Color Code "#RRGGBB" or color name "black".
        /// </summary>
        public string ResultColor { get; set; } = "#343a40";

        /// <summary>
        /// Block color for Results with Action. Use HTML like Hex Color Code "#RRGGBB" or color name "black".
        /// </summary>
        public string ResultActionColor { get; set; } = "#28a745";

        /// <summary>
        /// Block color for Actions. Use HTML like Hex Color Code "#RRGGBB" or color name "black".
        /// </summary>
        public string ActionColor { get; set; } = "#6c757d";

        /// <summary>
        /// Font color used in blocks. Use HTML like Hex Color Code "#RRGGBB" or color name "black".
        /// </summary>
        public string FontColor { get; set; } = "#ffffff";
    }
}
