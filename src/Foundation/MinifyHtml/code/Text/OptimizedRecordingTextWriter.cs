using HtmlAgilityPack;
using Sitecore.Foundation.MinifyHtml.Extensions;
using Sitecore.Mvc.Common;
using Sitecore.Mvc.Extensions;
using System.IO;

namespace Sitecore.Foundation.MinifyHtml.Text
{
    public class OptimizedRecordingTextWriter : RecordingTextWriter
    {
        /// <inheritdoc />
        public OptimizedRecordingTextWriter(TextWriter writer) : base(writer)
        {
        }

        /// <inheritdoc />
        public override void Flush()
        {
            var text = this.ToString();
            if (text.IsEmptyOrNull())
            {
                return;
            }
            var doc = new HtmlDocument();
            doc.LoadHtml(text);
            MinificationHelpers.ProcessScript(doc);
            MinificationHelpers.ProcessHtml(doc);

            text = doc.DocumentNode.OuterHtml;
            base.InnerWriter.Write(text);
            base.GetStringBuilder().Clear();
            base.FlushedText += text;
        }
    }
}