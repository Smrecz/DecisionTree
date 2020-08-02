using System;
using System.Linq;
using DecisionTree.Tree;

namespace DecisionTree.DotTreeExtensions
{
    public static class TreeExtensions
    {
        public static string ConvertToDotGraph<T, TResult>(this DecisionTreeBase<T, TResult> decisionTree, bool horizontal = false)
        {
            var graphDefinition = $"digraph G {{{Environment.NewLine}" +
            $"{(horizontal ? string.Empty : $"rankdir = LR;{Environment.NewLine}")}" +
            $"{decisionTree.Trunk().PrintNode()}" +
            "}";

            var deduplicatePaths = graphDefinition.Split(Environment.NewLine).Distinct();

            return string.Join(Environment.NewLine, deduplicatePaths);
        }
    }
}
