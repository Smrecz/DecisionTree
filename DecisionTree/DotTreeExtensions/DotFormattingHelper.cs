using System;
using System.Web;

namespace DecisionTree.DotTreeExtensions
{
    internal class DotFormattingHelper
    {
        public static string GetTableBody(string title, string color, string fontColor) =>
            GetTableBody(title, null, null, color, fontColor);

        public static string GetTableBody(string title, string conditionRow, string actionPartRow, string color, string fontColor) =>
            $"\"{title}\" [{GetActionStyle(color)} label = " +
                $"<<table {GetTableStyle(color)}>" +
                    "<tr>" +
                        $"<td {GetTitleCellStyle(color)}>" +
                            $"<font {GetFontStyle(fontColor)}>{HttpUtility.HtmlEncode(title)}</font>" +
                        "</td>" +
                    "</tr>" +
                conditionRow +
                actionPartRow +
                "</table>>" +
            $"]{Environment.NewLine}";

        public static string GetConditionRow(string condition, string fontColor) =>
            "<tr>" +
                "<td align = \"left\">" +
                    $"<font {GetFontStyle(fontColor)}>{HttpUtility.HtmlEncode(condition)}</font>" +
                "</td>" +
            "</tr>";

        public static string GetActionPartRow(string actionPart, string fontColor) =>
            "<tr>" +
                "<td align = \"left\">" +
                    $"<font {GetFontStyle(fontColor)}>{HttpUtility.HtmlEncode(actionPart)}</font>" +
                "</td>" +
            "</tr>";

        public static string GetLabel(string titleWithCounter, string label) =>
            GetLabel(titleWithCounter, label, string.Empty);

        public static string GetLabel(string titleWithCounter, string label, string actionDescription) =>
            $"\"{titleWithCounter}\" [label = \"{label}\"]{Environment.NewLine}{actionDescription}";

        public static string GetTitleWithCounter(int? counter, string title) =>
            $"[{counter}] {title}";

        public static string GetPath(string titleWithCounter, string childString) =>
            $"\"{titleWithCounter}\" -> {childString}";

        private static string GetActionStyle(string color) =>
            $"style = \"filled\" penwidth = 1 fillcolor = \"{color}\" fontname = \"Courier New\" shape = \"Mrecord\"";

        private static string GetTitleCellStyle(string color) =>
            $"bgcolor = \"{color}\" align = \"center\"";

        private static string GetTableStyle(string color) =>
            $"border = \"0\" cellborder = \"0\" cellpadding = \"3\" bgcolor = \"{color}\"";

        private static string GetFontStyle(string fontColor) =>
            $"color = \"{fontColor}\"";

        public static string GetSeparator(string fontColor) =>
            "<tr>" +
                $"<td bgcolor = \"{fontColor}\" cellpadding= \"1\">" +
                "</td>" +
            "</tr>";
    }
}
