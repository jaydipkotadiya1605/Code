using Sitecore.Diagnostics;
using Sitecore.Foundation.ItemResolver.Extensions;
using Sitecore.Mvc.Pipelines.Response.GetPageItem;
using System;

namespace Sitecore.Foundation.ItemResolver.Pipelines.Response.GetPageItem
{
    using Sitecore.Sites;

    /// <summary>
    /// Process GetPageItemProcessor pipeline
    /// </summary>
    public class GetResovedItem : GetPageItemProcessor
    {
        /// <summary>
        /// Processes the specified GetPageItem pipeline arguments.
        /// </summary>
        /// <param name="args">The pipeline arguments.</param>
        public override void Process(GetPageItemArgs args)
        {
            Assert.ArgumentNotNull(args, nameof(args));

            // Ignore requests if it should not be applied (see method for details).
            if (this.IgnoreRequest(args))
            {
                return;
            }

            // Get cache to determine if item has been resolved
            var isResolved = MainUtil.GetBool(Context.Items[Constants.CustomUrlResloverCacheKey], false);
            if (isResolved)
            {
                // Item has previously been resolved
                args.Result = Context.Item;
            }
        }

        /// <summary>
        /// Indicates whether the request should be ignored.
        /// </summary>
        /// <param name="args">The pipeline arguments.</param>
        /// <returns></returns>
        private bool IgnoreRequest(GetPageItemArgs args)
        {
            return Context.Site == null || Context.Domain == null || Context.Item == null || Context.Database == null
                || args.Result != null
                || Configuration.IgnoreSites.Count != 0 && Configuration.IgnoreSites.Contains(Context.Site.Name.ToLower())
                || Context.Domain.Name.Equals(Constants.Sitecore.DefaultDomain, StringComparison.InvariantCultureIgnoreCase) && Context.Site.DisplayMode != DisplayMode.Edit
                || Context.Database.Name.Equals(Constants.Sitecore.CoreDatabse, StringComparison.InvariantCultureIgnoreCase)
                || Context.Item.IsWilcard();
        }
    }
}