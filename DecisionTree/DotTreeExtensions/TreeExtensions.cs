using System;
using System.Linq;
using DecisionTree.Tree;

namespace DecisionTree.DotTreeExtensions
{
    public static class TreeExtensions
    {
        public static string ConvertToDotGraph<T>(this DecisionTreeBase<T> decisionTree, bool horizontal = false)
        {
            var newLine = Environment.NewLine;
            var graphDefinition = $"digraph G {{{newLine}" +
            $"{(horizontal ? $"rankdir = LR;{newLine}" : string.Empty)}" +
            $"{decisionTree.GetTrunk().InvokeChildPrint()}" +
            "}";

            var deduplicatePaths = graphDefinition.Split(newLine).Distinct();

            return string.Join(newLine, deduplicatePaths);
        }
    }
}
