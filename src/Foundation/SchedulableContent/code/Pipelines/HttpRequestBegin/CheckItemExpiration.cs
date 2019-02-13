using Sitecore.Diagnostics;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Pipelines.HttpRequest;
using Sitecore.Sites;
using System;

namespace Sitecore.Foundation.SchedulableContent.Pipelines.HttpRequestBegin
{
    /// <summary>
    /// Process HttpRequestProcessor pipeline
    /// </summary>
    public class CheckItemExpiration : HttpRequestProcessor
    {
        /// <summary>
        /// Processes the specified HttpRequest pipeline arguments.
        /// </summary>
        /// <param name="args">The HttpRequest arguments.</param>
        public override void Process(HttpRequestArgs args)
        {
            Assert.ArgumentNotNull(args, nameof(args));

            // Ignore requests if should not be applied (see method for details).
            if (this.IgnoreRequest(args))
            {
                return;
            }

            // Check if item is SchedulableContent
            var item = Context.Item;
            if (!item.IsDerived(FrasersContent.Templates.SchedulableContent.ID) || !item.FieldHasValue(FrasersContent.Templates.SchedulableContent.Fields.ExpiryDate))
            {
                return;
            }

            // Get Expiry Date
            var expiryDate = item.GetDateTime(FrasersContent.Templates.SchedulableContent.Fields.ExpiryDate);
            if (expiryDate < DateTime.Now)
            {
                return;
            }

            // Context is expired
            Context.Item = null;

            // Set cache indicate item has been resolved
            Context.Items[ItemResolver.Constants.CustomUrlResloverCacheKey] = true;
        }

        /// <summary>
        /// Indicates whether the request should be ignored.
        /// </summary>
        /// <param name="args">The pipeline arguments.</param>
        /// <returns></returns>
        private bool IgnoreRequest(HttpRequestArgs args)
        {
            return Context.Site == null || Context.Domain == null || Context.Item == null || Context.Database == null
                || ItemResolver.Configuration.IgnoreSites.Count != 0 && ItemResolver.Configuration.IgnoreSites.Contains(Context.Site.Name.ToLower())
                || args.Url.ItemPath.Length == 0
                || Context.Domain.Name.Equals(Constants.Sitecore.DefaultDomain, StringComparison.InvariantCultureIgnoreCase) && Context.Site.DisplayMode != DisplayMode.Edit
                || Context.Database.Name.Equals(Constants.Sitecore.CoreDatabse, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}