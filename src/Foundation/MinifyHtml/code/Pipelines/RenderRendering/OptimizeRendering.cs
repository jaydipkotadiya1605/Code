using Sitecore.Diagnostics;
using Sitecore.Foundation.MinifyHtml.Extensions;
using Sitecore.Foundation.MinifyHtml.Text;
using Sitecore.Mvc.Common;
using Sitecore.Mvc.Pipelines.Response.RenderRendering;

namespace Sitecore.Foundation.MinifyHtml.Pipelines.RenderRendering
{
    public class OptimizeRendering : RenderRenderingProcessor
    {
        public override void Process(RenderRenderingArgs args)
        {
            Assert.ArgumentNotNull(args, nameof(args));
            // Ignore requests if itemresolver should not be applied (see method for details).
            if (this.IgnoreRequest(args))
            {
                return;
            }

            this.StartRecording(args);
        }
        protected virtual void StartRecording(RenderRenderingArgs args)
        {
            var writer = MinificationHelpers.ShouldMinify() ? new OptimizedRecordingTextWriter(args.Writer) : new RecordingTextWriter(args.Writer);
            var item = new GenericDisposable(delegate
            {
                writer.Flush();
            });
            args.Disposables.Add(item);
            args.Writer = writer;
        }

        /// <summary>
        /// Indicates whether the request should be ignored.
        /// </summary>
        /// <param name="args">The pipeline arguments.</param>
        /// <returns></returns>
        private bool IgnoreRequest(RenderRenderingArgs args)
        {
            return Context.Site == null || Context.Domain == null || Context.Item == null || Context.Database == null
                   || ItemResolver.Configuration.IgnoreSites.Count != 0 && ItemResolver.Configuration.IgnoreSites.Contains(Context.Site.Name.ToLower())
                   || args.Rendered;
        }
    }
}