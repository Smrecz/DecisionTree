using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using DecisionTree.Decisions;
using DecisionTree.Exceptions;

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

        private static readonly MethodInfo PrintActionMethod =
            typeof(NodePrintableExtensions)
                .GetMethod(nameof(PrintAction));

        private static readonly string DecisionNodeName = typeof(DecisionNode<,>).Name;
        private static readonly string DecisionResultName = typeof(DecisionResult<>).Name;
        private static readonly string DecisionActionName = typeof(DecisionAction<>).Name;

        private const string DefaultOptionText = "#default_option";
        private const string ActionCellStyle = "align=\"left\"";
        private const string ActionPartRegexPattern = "(?=\\.[^\\)]+?\\([^\\)]+\\))";
        private const string FontStyle = "color=\"white\"";
        private const string Separator = "<tr><td bgcolor=\"white\" cellpadding=\"1\"></td></tr>";
        private static int? _internalCounter = 0;

        public static string Print<T>(this IDecision<T> decision, bool useCounter, string key = null)
        {
            _internalCounter = useCounter ? 0 : (int?)null;

            return decision.InvokeChildPrint(key);
        }

        private static string InvokeChildPrint<T>(this IDecision<T> decision, string key = null)
        {
            var genericTypes = decision.GetType().GenericTypeArguments;
            var decisionName = decision.GetType().Name;

            _internalCounter++;

            var printParams = new object[] { decision, _internalCounter, key };

            if (decisionName == DecisionResultName)
                return (string)PrintResultMethod
                    .MakeGenericMethod(genericTypes)
                    .Invoke(decision, printParams);

            if (decisionName == DecisionNodeName)
                return (string)PrintNodeMethod
                    .MakeGenericMethod(genericTypes)
                    .Invoke(decision, printParams);

            if (decisionName == DecisionActionName)
                return (string)PrintActionMethod
                    .MakeGenericMethod(genericTypes)
                    .Invoke(decision, printParams);

            throw new NotPrintableTypeException($"Printing of type {decision.GetType().Name} not supported.");
        }

        public static string PrintNode<T, TResult>(this DecisionNode<T, TResult> node, int? counter, string label = null)
        {
            var printResult = string.Empty;

            var condition = node.Condition.ToString();
            var titleWithCounter = AddCounter(counter, node.Title);

            if (label != null)
                printResult += $"\"{titleWithCounter}\" [label = \"{label}\"]{Environment.NewLine}";

            foreach (var (key, decision) in node.Paths)
                printResult += $"\"{titleWithCounter}\" -> {InvokeChildPrint(decision, key.ToString())}";

            if (node.DefaultPath != null)
                printResult += $"\"{titleWithCounter}\" -> {InvokeChildPrint(node.DefaultPath, DefaultOptionText)}";

            printResult += node.Action != null
                ? GetHtmlTable(node.Action.ToString(), condition, titleWithCounter, TitleStyle.DecisionAction)
                : GetHtmlTable(string.Empty, condition, titleWithCounter, TitleStyle.Decision);

            return printResult;
        }

        public static string PrintResult<T>(this DecisionResult<T> node, int? counter, string label)
        {
            var titleWithCounter = AddCounter(counter, node.Title);

            var actionDescription = node.Action != null
                ? GetHtmlTable(node.Action.ToString(), null, titleWithCounter, TitleStyle.ResultAction)
                : GetHtmlTable(string.Empty,null, titleWithCounter, TitleStyle.Result);

            return $"\"{titleWithCounter}\" [label = \"{label}\"]{Environment.NewLine}{actionDescription}";
        }

        public static string PrintAction<T>(this DecisionAction<T> node, int? counter, string label)
        {
            var printResult = string.Empty;

            var titleWithCounter = AddCounter(counter, node.Title);

            if (label != null)
                printResult += $"\"{titleWithCounter}\" [label = \"{label}\"]{Environment.NewLine}";

            if (node.Path != null)
                printResult += $"\"{titleWithCounter}\" -> {InvokeChildPrint(node.Path, label)}";

            printResult += GetHtmlTable(node.Action.ToString(), null, titleWithCounter, TitleStyle.Action);

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
                        condition+
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
                _ => throw new ArgumentOutOfRangeException(nameof(titleStyle), titleStyle, null)
            };
    }
}
