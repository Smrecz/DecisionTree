using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using DecisionTree.Decisions.DecisionsBase;
using DecisionTree.DotTreeExtensions.Dto;
using DecisionTree.Exceptions;

namespace DecisionTree.DotTreeExtensions
{
    public static class DecisionExtensions
    {
        private const string DefaultPathText = "#default_path";
        private const string NullPathText = "#null_path";
        private const string ActionPartRegexPattern = "(?=\\.[^\\)]+?\\([^\\)]+\\))";

        private static readonly Dictionary<Type, MethodInfo> ExtensionBindingDictionary =
            new Dictionary<Type, MethodInfo>
            {
                {typeof(IDecisionNode<,>), GetPrivateStaticMethodInfo(nameof(PrintNode))},
                {typeof(IDecisionResult<>), GetPrivateStaticMethodInfo(nameof(PrintResult))},
                {typeof(IDecisionAction<>), GetPrivateStaticMethodInfo(nameof(PrintAction))}
            };

        public static string Print<T>(this IDecision<T> decision, GraphOptions options) => 
            decision.InvokeChildPrint(new GraphConfig(options));

        private static MethodInfo GetPrivateStaticMethodInfo(string name) => typeof(DecisionExtensions)
            .GetMethod(name, BindingFlags.NonPublic | BindingFlags.Static);

        private static string InvokeChildPrint<T>(this IDecision<T> decision, GraphConfig graphConfig) =>
            InvokeChildPrint(decision, graphConfig, null);

        private static string InvokeChildPrint<T>(this IDecision<T> decision, GraphConfig graphConfig, string label)
        {
            var implementedInterface = GetImplementedPrintableInterface(decision);

            graphConfig.NodeId?.Increment();

            var printParams = new object[] { decision, graphConfig, label };

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
                printResult += DotFormattingHelper
                    .GetLabel(titleWithCounter, label);

            foreach (var (key, decision) in node.Paths)
                printResult += DotFormattingHelper
                    .GetPath(titleWithCounter, decision.InvokeChildPrint(graphConfig, key.ToString()));

            if (node.NullPath != null)
                printResult += DotFormattingHelper
                    .GetPath(titleWithCounter, node.NullPath.InvokeChildPrint(graphConfig, NullPathText));

            if (node.DefaultPath != null)
                printResult += DotFormattingHelper
                    .GetPath(titleWithCounter, node.DefaultPath.InvokeChildPrint(graphConfig, DefaultPathText));

            printResult += node.Action != null
                ? GetHtmlTable(node.Action.ToString(), graphConfig, condition, titleWithCounter, StyleElement.DecisionAction)
                : GetHtmlTable(string.Empty, graphConfig, condition, titleWithCounter, StyleElement.Decision);

            return printResult;
        }

        private static string PrintResult<T>(this IDecisionResult<T> result, GraphConfig graphConfig, string label)
        {
            var titleWithCounter = AddCounter(graphConfig.NodeId?.Counter, result.Title);

            var actionDescription = result.Action != null
                ? GetHtmlTable(result.Action.ToString(), graphConfig, titleWithCounter, StyleElement.ResultAction)
                : GetHtmlTable(string.Empty, graphConfig, titleWithCounter, StyleElement.Result);

            return DotFormattingHelper
                .GetLabel(titleWithCounter, label, actionDescription);
        }

        private static string PrintAction<T>(this IDecisionAction<T> action, GraphConfig graphConfig, string label)
        {
            var printResult = string.Empty;

            var titleWithCounter = AddCounter(graphConfig.NodeId?.Counter, action.Title);

            if (label != null)
                printResult += DotFormattingHelper
                    .GetLabel(titleWithCounter, label);

            if (action.Path != null)
                printResult += DotFormattingHelper
                    .GetPath(titleWithCounter, action.Path.InvokeChildPrint(graphConfig, label));

            printResult += GetHtmlTable(action.Action.ToString(), graphConfig, titleWithCounter, StyleElement.Action);

            return printResult;
        }

        private static string GetHtmlTable(string action, GraphConfig graphConfig, string title, StyleElement styleElement) =>
            GetHtmlTable(action, graphConfig, null, title, styleElement);

        private static string GetHtmlTable(string action, GraphConfig graphConfig, string condition, string title, StyleElement styleElement)
        {
            var color = graphConfig.GetColor(styleElement);
            var fontColor = graphConfig.GetColor(StyleElement.Font);

            if (graphConfig.TitleOnly)
                return DotFormattingHelper.GetTableBody(title, color, fontColor);

            var conditionRow = GetConditionRow(condition, fontColor);
            var actionPartRow = GetActionPartRow(action, fontColor);

            return DotFormattingHelper.GetTableBody(title, conditionRow, actionPartRow, color, fontColor);
        }

        private static string GetConditionRow(string condition, string fontColor)
        {
            var conditionRow = string.Empty;

            if (condition == null) 
                return conditionRow;

            conditionRow += DotFormattingHelper
                .GetSeparator(fontColor);

            conditionRow += DotFormattingHelper
                .GetConditionRow(condition, fontColor);

            return conditionRow;
        }

        private static string GetActionPartRow(string action, string fontColor)
        {
            var actionPartRow = string.Empty;

            var actionParts = Regex
                .Split(action, ActionPartRegexPattern)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            if (actionParts.Any())
                actionPartRow += DotFormattingHelper
                    .GetSeparator(fontColor);

            foreach (var actionPart in actionParts)
                actionPartRow += DotFormattingHelper
                    .GetActionPartRow(actionPart, fontColor);

            return actionPartRow;
        }

        private static string AddCounter(int? counter, string title) =>
            counter != null
                ? DotFormattingHelper.GetTitleWithCounter(counter, title)
                : title;
    }
}
