using DecisionTree.Decisions;
using System;
using System.Linq;
using System.Reflection;

namespace DecisionTree.DotTreeExtensions.DecisionExtensions
{
    public static class NodePrintableExtensions
    {
        private static readonly MethodInfo PrintNodeMethod = 
                    typeof(NodePrintableExtensions)
                    .GetMethod(nameof(PrintNode));

        private static readonly MethodInfo PrintResultMethod =
                    typeof(NodePrintableExtensions)
                    .GetMethod(nameof(PrintResult));

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
                printResult += $"{condition} -> {InvokeChildPrint(node.DefaultPath, "#default_option")}";

            return printResult;
        }

        public static string PrintResult<T>(this DecisionResult<T> node, string label) =>
            $"\"{node.Title}\" [label = \"{label}\"]{Environment.NewLine}";

        private static object InvokeChildPrint<T>(IDecision<T> decision, string key)
        {
            var genericTypes = decision.GetType().GenericTypeArguments;
            var printParams = new object[] { decision, key };

            return genericTypes.Count() switch
            {
                1 => PrintResultMethod
                        .MakeGenericMethod(genericTypes)
                        .Invoke(decision, printParams),
                2 => PrintNodeMethod
                        .MakeGenericMethod(genericTypes)
                        .Invoke(decision, printParams),
                _ => throw new ArgumentException("Unexpected extension count")
            };
        }       
    }
}
