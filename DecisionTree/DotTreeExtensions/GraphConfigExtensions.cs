using DecisionTree.DotTreeExtensions.Dto;

namespace DecisionTree.DotTreeExtensions
{
    internal static class GraphConfigExtensions
    {
        public static string GetColor(this GraphConfig graphConfig, StyleElement styleElement) =>
            styleElement switch
            {
                StyleElement.Decision => graphConfig.Style.DecisionColor,
                StyleElement.DecisionAction => graphConfig.Style.DecisionActionColor,
                StyleElement.Result => graphConfig.Style.ResultColor,
                StyleElement.ResultAction => graphConfig.Style.ResultActionColor,
                StyleElement.Action => graphConfig.Style.ActionColor,
                StyleElement.Font => graphConfig.Style.FontColor
            };
    }
}
