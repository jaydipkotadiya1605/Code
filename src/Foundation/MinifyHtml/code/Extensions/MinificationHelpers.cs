using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace Sitecore.Foundation.MinifyHtml.Extensions
{
    public static class MinificationHelpers
    {
        public static bool ShouldMinify() => Configuration.Settings.GetBoolSetting(Constants.MinifyResponseMarkupConfigKey, false);

        public static void ProcessScript(HtmlDocument doc)
        {
            var scriptRegex = new Regex(@"<script[^>]*>[\w|\t|\r|\W]*?</script>", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
            doc.LoadHtml(scriptRegex.Replace(doc.DocumentNode.OuterHtml, delegate (Match m)
            {
                var groups = m.Groups;
                return groups[0].Value.Contains(" src=") ? groups[0].Value : JsMinifier.MinifyJs(groups[0].Value);
            }));
        }

        public static void ProcessHtml(HtmlDocument doc, bool removeWhitespaces = true, bool removeLineBreaks = true, bool removeHtmlComments = true)
        {
            if (removeWhitespaces)
            {
                doc.LoadHtml(HtmlHelpers.RemoveWhiteSpaces(doc.DocumentNode.OuterHtml));
                doc.LoadHtml(HtmlHelpers.RemoveTabSpace(doc.DocumentNode.OuterHtml));
                doc.LoadHtml(HtmlHelpers.RemoveSpaceBeforeTag(doc.DocumentNode.OuterHtml));
                doc.LoadHtml(HtmlHelpers.RemoveSpaceAfterTag(doc.DocumentNode.OuterHtml));
            }

            if (removeLineBreaks)
            {
                doc.LoadHtml(HtmlHelpers.RemoveLineBreaks(doc.DocumentNode.OuterHtml));
                doc.LoadHtml(HtmlHelpers.RemoveNewLine(doc.DocumentNode.OuterHtml));
            }

            if (removeHtmlComments)
            {
                doc.LoadHtml(HtmlHelpers.RemoveHtmlComments(doc.DocumentNode.OuterHtml));
            }
        }
    }
}