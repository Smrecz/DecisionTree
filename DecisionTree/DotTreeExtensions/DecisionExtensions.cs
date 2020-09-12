using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using DecisionTree.Decisions.DecisionsBase;
using DecisionTree.Exceptions;

namespace DecisionTree.DotTreeExtensions
{
    public static class DecisionExtensions
    {
        private static readonly Dictionary<Type, MethodInfo> ExtensionBindingDictionary = new Dictionary<Type, MethodInfo>
        {
            {typeof(IDecisionNode<,>), typeof(DecisionExtensions).GetMethod(nameof(PrintNode))},
            {typeof(IDecisionResult<>), typeof(DecisionExtensions).GetMethod(nameof(PrintResult))},
            {typeof(IDecisionAction<>), typeof(DecisionExtensions).GetMethod(nameof(PrintAction))}
        };

        private const string DefaultPathText = "#default_path";
        private const string NullPathText = "#null_path";
        private const string ActionCellStyle = "align=\"left\"";
        private const string ActionPartRegexPattern = "(?=\\.[^\\)]+?\\([^\\)]+\\))";
        private const string FontStyle = "color=\"white\"";
        private const string Separator = "<tr><td bgcolor=\"white\" cellpadding=\"1\"></td></tr>";

        public static string Print<T>(this IDecision<T> decision, bool useCounter, string key = null)
        {
            var nodeId = useCounter ? new NodeId() : null;

            return decision.InvokeChildPrint(nodeId, key);
        }

        private static string InvokeChildPrint<T>(this IDecision<T> decision, NodeId nodeId, string key = null)
        {
            var implementedInterface = GetImplementedPrintableInterface(decision);

            nodeId?.Increment();

            var printParams = new object[] { decision, nodeId, key };

            return (string)ExtensionBindingDictionary[implementedInterface.GetGenericTypeDefinition()]
                .MakeGenericMethod(implementedInterface.GenericTypeArguments)
                .Invoke(decision, printParams);
        }

        private static Type GetImplementedPrintableInterface<T>(IDecision<T> decision) =>
            decision
                .GetType()
                .GetInterfaces()
                .Where(type => type.IsGenericType)
                .FirstOrDefault(type => ExtensionBindingDictionary.Keys.Contains(type.GetGenericTypeDefinition())) 
            ?? throw new NotPrintableTypeException($"Printing of type {decision.GetType().Name} not supported.");

        public static string PrintNode<T, TResult>(this IDecisionNode<T, TResult> node, NodeId nodeId, string label = null)
        {
            var printResult = string.Empty;

            var condition = node.Condition.ToString();
            var titleWithCounter = AddCounter(nodeId?.Counter, node.Title);

            if (label != null)
                printResult += $"\"{titleWithCounter}\" [label = \"{label}\"]{Environment.NewLine}";

            foreach (var (key, decision) in node.Paths)
                printResult += $"\"{titleWithCounter}\" -> {decision.InvokeChildPrint(nodeId, key.ToString())}";

            if (node.NullPath != null)
                printResult += $"\"{titleWithCounter}\" -> {node.NullPath.InvokeChildPrint(nodeId, NullPathText)}";

            if (node.DefaultPath != null)
                printResult += $"\"{titleWithCounter}\" -> {node.DefaultPath.InvokeChildPrint(nodeId, DefaultPathText)}";

            printResult += node.Action != null
                ? GetHtmlTable(node.Action.ToString(), condition, titleWithCounter, TitleStyle.DecisionAction)
                : GetHtmlTable(string.Empty, condition, titleWithCounter, TitleStyle.Decision);

            return printResult;
        }

        public static string PrintResult<T>(this IDecisionResult<T> result, NodeId nodeId, string label)
        {
            var titleWithCounter = AddCounter(nodeId?.Counter, result.Title);

            var actionDescription = result.Action != null
                ? GetHtmlTable(result.Action.ToString(), null, titleWithCounter, TitleStyle.ResultAction)
                : GetHtmlTable(string.Empty, null, titleWithCounter, TitleStyle.Result);

            return $"\"{titleWithCounter}\" [label = \"{label}\"]{Environment.NewLine}{actionDescription}";
        }

        public static string PrintAction<T>(this IDecisionAction<T> action, NodeId nodeId, string label)
        {
            var printResult = string.Empty;

            var titleWithCounter = AddCounter(nodeId?.Counter, action.Title);

            if (label != null)
                printResult += $"\"{titleWithCounter}\" [label = \"{label}\"]{Environment.NewLine}";

            if (action.Path != null)
                printResult += $"\"{titleWithCounter}\" -> {action.Path.InvokeChildPrint(nodeId, label)}";

            printResult += GetHtmlTable(action.Action.ToString(), null, titleWithCounter, TitleStyle.Action);

            return printResult;
        }

        private static string GetHtmlTable(string action, string condition, string title, TitleStyle titleStyle)
        {
            var style = GetTitleStyle(titleStyle);
            var actionStyle = GetActionStyle(style);

            var actionPartRow = string.Empty;
            var conditionRow = string.Empty;

            if (condition != null)
            {
                conditionRow += Separator;
                conditionRow += GetConditionRow(condition);
            }

            var actionParts = Regex
                .Split(action, ActionPartRegexPattern)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            if (actionParts.Any())
                actionPartRow += Separator;

            foreach (var actionPart in actionParts)
                actionPartRow += GetActionPartRow(actionPart);

            var table = GetTableBody(title, conditionRow, actionPartRow, style);

            return $"\"{title}\" [{actionStyle} label = {table}]{Environment.NewLine}";
        }

        private static string AddCounter(int? counter, string title)
        {
            return counter != null
                ? $"[{counter}] {title}"
                : title;
        }

        private static string GetTableBody(string title, string condition, string actionPartRow, string style)
        {
            var tableStyle = GetTableStyle(style);
            var titleCellStyle = GetTitleCellStyle(style);

            return $"<<table {tableStyle}>" +
                        "<tr>" +
                            $"<td {titleCellStyle}>" +
                                $"<font {FontStyle}>{HttpUtility.HtmlEncode(title)}</font>" +
                            "</td>" +
                        "</tr>" +
                        condition +
                        actionPartRow +
                   "</table>>";
        }

        private static string GetConditionRow(string condition) =>
            "<tr>" +
                $"<td {ActionCellStyle}>" +
                    $"<font {FontStyle}>{HttpUtility.HtmlEncode(condition)}</font>" +
                "</td>" +
            "</tr>";

        private static string GetActionPartRow(string actionPart) =>
            "<tr>" +
                $"<td {ActionCellStyle}>" +
                    $"<font {FontStyle}>{HttpUtility.HtmlEncode(actionPart)}</font>" +
                "</td>" +
            "</tr>";

        private static string GetTitleCellStyle(string style) =>
            $"bgcolor=\"{style}\" align=\"center\"";

        private static string GetTableStyle(string style) =>
            $"border=\"0\" cellborder=\"0\" cellpadding=\"3\" bgcolor=\"{style}\"";

        private static string GetActionStyle(string style) =>
            $"style = \"filled\" penwidth = 1 fillcolor = \"{style}\" fontname = \"Courier New\" shape = \"Mrecord\"";

        private static string GetTitleStyle(TitleStyle titleStyle) =>
            titleStyle switch
            {
                TitleStyle.Decision => "#17a2b8",
                TitleStyle.DecisionAction => "#007bff",
                TitleStyle.Result => "#343a40",
                TitleStyle.ResultAction => "#28a745",
                TitleStyle.Action => "#6c757d",
                _ => "#343a40"
            };
    }
}
