using System;
using System.Linq;
using DecisionTree.Tree;

namespace DecisionTree.DotTreeExtensions
{
    public static class TreeExtensions
    {
        public static string ConvertToDotGraph<T>(this DecisionTreeBase<T> decisionTree) => 
            decisionTree.ConvertToDotGraph(new GraphOptions());

        public static string ConvertToDotGraph<T>(this DecisionTreeBase<T> decisionTree, GraphOptions options)
        {
            var newLine = Environment.NewLine;

            var graphDefinition = $"digraph G {{{newLine}" +
                                  $"{(options.IsHorizontal ? $"rankdir = LR;{newLine}" : string.Empty)}" +
                                  $"{decisionTree.GetTrunk().Print(options)}" +
                                  "}";

            var deduplicatePaths = graphDefinition.Split(newLine).Distinct();

            return string.Join(newLine, deduplicatePaths);
        }
    }
}
