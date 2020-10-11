using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using DecisionTree.Decisions.DecisionsBase;
using DecisionTree.DotTreeExtensions.Dto;
using DecisionTree.Exceptions;

namespace DecisionTree.DotTreeExtensions
{
    public static class DecisionExtensions
    {
        private static readonly Dictionary<Type, MethodInfo> ExtensionBindingDictionary =
            new Dictionary<Type, MethodInfo>
            {
                {typeof(IDecisionNode<,>), GetPrivateStaticMethodInfo(nameof(PrintNode))},
                {typeof(IDecisionResult<>), GetPrivateStaticMethodInfo(nameof(PrintResult))},
                {typeof(IDecisionAction<>), GetPrivateStaticMethodInfo(nameof(PrintAction))}
            };

        private static MethodInfo GetPrivateStaticMethodInfo(string name) => typeof(DecisionExtensions)
            .GetMethod(name, BindingFlags.NonPublic | BindingFlags.Static);

        private static readonly Dictionary<TitleStyle, string> TitleStyleDictionary = 
            new Dictionary<TitleStyle, string>
            {
                {TitleStyle.Decision, "#17a2b8"},
                {TitleStyle.DecisionAction, "#007bff"},
                {TitleStyle.Result, "#343a40"},
                {TitleStyle.ResultAction, "#28a745"},
                {TitleStyle.Action, "#6c757d"}
            };

        private const string DefaultPathText = "#default_path";
        private const string NullPathText = "#null_path";
        private const string ActionCellStyle = "align=\"left\"";
        private const string ActionPartRegexPattern = "(?=\\.[^\\)]+?\\([^\\)]+\\))";
        private const string FontStyle = "color=\"white\"";
        private const string Separator = "<tr><td bgcolor=\"white\" cellpadding=\"1\"></td></tr>";

        public static string Print<T>(this IDecision<T> decision, GraphOptions options)
        {
            var nodeId = options.UseUniquePaths ? new NodeId() : null;
            var graphConfig = new GraphConfig(nodeId, options.TitleOnly);

            return decision.InvokeChildPrint(graphConfig);
        }

        private static string InvokeChildPrint<T>(this IDecision<T> decision, GraphConfig graphConfig) =>
            InvokeChildPrint(decision, graphConfig, null);

        private static string InvokeChildPrint<T>(this IDecision<T> decision, GraphConfig graphConfig, string key)
        {
            var implementedInterface = GetImplementedPrintableInterface(decision);

            graphConfig.NodeId?.Increment();

            var printParams = new object[] { decision, graphConfig, key };

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
            ?? throw new NotPrintableTypeException(decision.GetType());

        private static string PrintNode<T, TResult>(this IDecisionNode<T, TResult> node, GraphConfig graphConfig, string label = null)
        {
            var printResult = string.Empty;

            var condition = node.Condition.ToString();
            var titleWithCounter = AddCounter(graphConfig.NodeId?.Counter, node.Title);

            if (label != null)
                printResult += $"\"{titleWithCounter}\" [label = \"{label}\"]{Environment.NewLine}";

            foreach (var (key, decision) in node.Paths)
                printResult += $"\"{titleWithCounter}\" -> {decision.InvokeChildPrint(graphConfig, key.ToString())}";

            if (node.NullPath != null)
                printResult += $"\"{titleWithCounter}\" -> {node.NullPath.InvokeChildPrint(graphConfig, NullPathText)}";

            if (node.DefaultPath != null)
                printResult += $"\"{titleWithCounter}\" -> {node.DefaultPath.InvokeChildPrint(graphConfig, DefaultPathText)}";

            printResult += node.Action != null
                ? GetHtmlTable(node.Action.ToString(), graphConfig.TitleOnly, condition, titleWithCounter, TitleStyle.DecisionAction)
                : GetHtmlTable(string.Empty, graphConfig.TitleOnly, condition, titleWithCounter, TitleStyle.Decision);

            return printResult;
        }

        private static string PrintResult<T>(this IDecisionResult<T> result, GraphConfig graphConfig, string label)
        {
            var titleWithCounter = AddCounter(graphConfig.NodeId?.Counter, result.Title);

            var actionDescription = result.Action != null
                ? GetHtmlTable(result.Action.ToString(), graphConfig.TitleOnly, titleWithCounter, TitleStyle.ResultAction)
                : GetHtmlTable(string.Empty, graphConfig.TitleOnly, titleWithCounter, TitleStyle.Result);

            return $"\"{titleWithCounter}\" [label = \"{label}\"]{Environment.NewLine}{actionDescription}";
        }

        private static string PrintAction<T>(this IDecisionAction<T> action, GraphConfig graphConfig, string label)
        {
            var printResult = string.Empty;

            var titleWithCounter = AddCounter(graphConfig.NodeId?.Counter, action.Title);

            if (label != null)
                printResult += $"\"{titleWithCounter}\" [label = \"{label}\"]{Environment.NewLine}";

            if (action.Path != null)
                printResult += $"\"{titleWithCounter}\" -> {action.Path.InvokeChildPrint(graphConfig, label)}";

            printResult += GetHtmlTable(action.Action.ToString(), graphConfig.TitleOnly, titleWithCounter, TitleStyle.Action);

            return printResult;
        }

        private static string GetHtmlTable(string action, bool titleOnly, string title, TitleStyle titleStyle) =>
            GetHtmlTable(action, titleOnly, null, title, titleStyle);

        private static string GetHtmlTable(string action, bool titleOnly, string condition, string title, TitleStyle titleStyle)
        {
            var style = TitleStyleDictionary[titleStyle];
            var actionStyle = GetActionStyle(style);

            if (titleOnly)
                return $"\"{title}\" [{actionStyle} label = {GetTableBody(title, style)}]{Environment.NewLine}";

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

        private static string GetTableBody(string title, string style) =>
            GetTableBody(title, null, null, style);

        private static string GetTableBody(string title, string conditionRow, string actionPartRow, string style) =>
            $"<<table {GetTableStyle(style)}>" +
                 "<tr>" +
                     $"<td {GetTitleCellStyle(style)}>" +
                         $"<font {FontStyle}>{HttpUtility.HtmlEncode(title)}</font>" +
                     "</td>" +
                 "</tr>" +
                 conditionRow +
                 actionPartRow +
            "</table>>";

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
    }
}
