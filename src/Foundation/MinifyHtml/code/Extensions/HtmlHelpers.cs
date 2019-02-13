using System.Text.RegularExpressions;

namespace Sitecore.Foundation.MinifyHtml.Extensions
{
    public static class HtmlHelpers
    {
        public static string RemoveTabSpace(string markup) => Regex.Replace(markup, "\t", string.Empty, RegexOptions.Compiled | RegexOptions.Multiline);

        public static string RemoveLineBreaks(string markup) => Regex.Replace(markup, "\\n", string.Empty, RegexOptions.Compiled | RegexOptions.Multiline);

        public static string RemoveNewLine(string markup) => Regex.Replace(markup, "\\r", string.Empty, RegexOptions.Compiled | RegexOptions.Multiline);

        public static string RemoveWhiteSpaces(string markup) => Regex.Replace(markup, @"^\s+", string.Empty, RegexOptions.Compiled | RegexOptions.Multiline);

        public static string RemoveHtmlComments(string markup) => Regex.Replace(markup, @"<!--(?!\[).*?(?!<\])-->", string.Empty, RegexOptions.Compiled | RegexOptions.Multiline);

        public static string RemoveSpaceBeforeTag(string markup) => Regex.Replace(markup, @"\s+<", " <", RegexOptions.Compiled | RegexOptions.Multiline);

        public static string RemoveSpaceAfterTag(string markup) => Regex.Replace(markup, @">\s+", "> ", RegexOptions.Compiled | RegexOptions.Multiline);
    }
}