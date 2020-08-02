using System;
using System.Reflection;
using DecisionTree.Decisions;

namespace DecisionTree.DotTreeExtensions
{
    public static class NodePrintableExtensions
    {
        private static readonly MethodInfo PrintNodeMethod =
                    typeof(NodePrintableExtensions)
                    .GetMethod(nameof(PrintNode));

        private static readonly MethodInfo PrintResultMethod =
                    typeof(NodePrintableExtensions)
                    .GetMethod(nameof(PrintResult));

        private static readonly string DecisionNodeName = typeof(DecisionNode<,>).Name;
        private static readonly string DecisionResultName = typeof(DecisionResult<>).Name;

        private const string DefaultOptionText = "#default_option";

        public static string PrintNode<T, TResult>(this DecisionNode<T, TResult> node, string label = null)
        {
            var printResult = string.Empty;

            //Escape quotation marks as they are special symbols in DOT language.
            var condition = $"\"{node.Condition.ToString().Replace("\"", "\\\"")}\"";

            if (label != null)
                printResult += $"{condition} [label = \"{label}\"]{Environment.NewLine}";

            foreach (var (key, decision) in node.Paths)
                printResult += $"{condition} -> {InvokeChildPrint(decision, key.ToString())}";

            if (node.DefaultPath != null)
                printResult += $"{condition} -> {InvokeChildPrint(node.DefaultPath, DefaultOptionText)}";

            return printResult;
        }

        public static string PrintResult<T>(this DecisionResult<T> node, string label) =>
            $"\"{node.Title}\" [label = \"{label}\"]{Environment.NewLine}";

        private static object InvokeChildPrint<T>(IDecision<T> decision, string key)
        {
            var genericTypes = decision.GetType().GenericTypeArguments;
            var printParams = new object[] { decision, key };
            var decisionName = decision.GetType().Name;

            if (decisionName == DecisionResultName)
                return PrintResultMethod
                    .MakeGenericMethod(genericTypes)
                    .Invoke(decision, printParams);

            if (decisionName == DecisionNodeName)
                return PrintNodeMethod
                    .MakeGenericMethod(genericTypes)
                    .Invoke(decision, printParams);

            throw new ArgumentException("Unexpected extension count.", nameof(decision));
        }
    }
}
