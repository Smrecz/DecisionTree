using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
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
        private const string ActionCellStyle = "align=\"left\"";
        private const string ActionPartRegexPattern = "(?=\\.[^\\)]+?\\([^\\)]+\\))";
        private const string FontStyle = "color=\"white\"";
        private const string ActionUnderscore = "<tr><td bgcolor=\"white\" cellpadding=\"1\"></td></tr>";

        public static string PrintNode<T, TResult>(this DecisionNode<T, TResult> node, string label = null)
        {
            var printResult = string.Empty;

            //Escape quotation marks as they are special symbols in DOT language.
            var condition = EscapeQuotation(node.Condition.ToString());

            if (label != null)
                printResult += $"\"{condition}\" [label = \"{label}\"]{Environment.NewLine}";

            foreach (var (key, decision) in node.Paths)
                printResult += $"\"{condition}\" -> {InvokeChildPrint(decision, key.ToString())}";

            if (node.DefaultPath != null)
                printResult += $"\"{condition}\" -> {InvokeChildPrint(node.DefaultPath, DefaultOptionText)}";

            printResult += node.Action != null
                ? GetHtmlTable(node.Action.ToString(), condition, TitleStyle.DecisionAction)
                : GetHtmlTable(string.Empty, condition, TitleStyle.Decision);

            return printResult;
        }

        public static string PrintResult<T>(this DecisionResult<T> node, string label)
        {
            var actionDescription = node.Action != null
                ? GetHtmlTable(node.Action.ToString(), node.Title, TitleStyle.ResultAction)
                : GetHtmlTable(string.Empty, node.Title, TitleStyle.Result);

            return $"\"{node.Title}\" [label = \"{label}\"]{Environment.NewLine}{actionDescription}";
        }
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

        private static string GetHtmlTable(string action, string title, TitleStyle titleStyle)
        {
            var style = GetTitleStyle(titleStyle);
            var actionStyle = GetActionStyle(style);

            var actionPartRow = string.Empty;

            var actionParts = Regex
                .Split(action, ActionPartRegexPattern)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            if (actionParts.Any())
                actionPartRow += ActionUnderscore;

            foreach (var actionPart in actionParts)
                actionPartRow += GetActionPartRow(actionPart);

            var table = GetTableBody(title, actionPartRow, style);

            return $"\"{title}\" [{actionStyle} label = {table}]{Environment.NewLine}";
        }

        private static string GetTableBody(string title, string actionPartRow, string style)
        {
            var tableStyle = GetTableStyle(style);
            var titleCellStyle = GetTitleCellStyle(style);

            return $"<<table {tableStyle}>" +
                        "<tr>" +
                            $"<td {titleCellStyle}>" +
                                $"<font {FontStyle}>{HttpUtility.HtmlEncode(title)}</font>" +
                            "</td>" +
                        "</tr>" +
                        actionPartRow +
                   "</table>>";
        }

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
                TitleStyle.Result => "#6c757d",
                TitleStyle.ResultAction => "#28a745",
                _ => throw new ArgumentOutOfRangeException(nameof(titleStyle), titleStyle, null)
            };

        private static string EscapeQuotation(string value) => 
            value.Replace("\"", "\\\"");
 
    }
}
