using Sitecore.Diagnostics;
using Sitecore.Foundation.ItemResolver.Extensions;
using Sitecore.Foundation.ItemResolver.Providers;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Pipelines.HttpRequest;
using Sitecore.Sites;
using System;

namespace Sitecore.Foundation.ItemResolver.Pipelines.HttpRequestBegin
{
    /// <summary>
    /// Process HttpRequestProcessor pipeline
    /// </summary>
    public class ResolveItemByUrl : HttpRequestProcessor
    {
        /// <summary>
        /// Processes the specified HttpRequest pipeline arguments.
        /// </summary>
        /// <param name="args">The HttpRequest arguments.</param>
        public override void Process(HttpRequestArgs args)
        {
            Assert.ArgumentNotNull(args, nameof(args));

            // Ignore requests if itemresolver should not be applied (see method for details).
            if (this.IgnoreRequest(args))
            {
                return;
            }

            // Resolve Wildcard Item
            if (Context.Item.IsWilcard())
            {
                // Get routes for item resolver
                var routes = WildcardManager.Current?.GetWildcardRouteForItemResolver(Context.Item, Context.Site);

                 // Reslove item by url and routes
                var result = WildcardItemResolver.Current?.ResolveItem(Context.Item, args.Url.FilePath, routes);

                // Set result to context item
                Context.Item = result;

                // Set cache indicate item has been resolved
                Context.Items[Constants.CustomUrlResloverCacheKey] = true;
            }
            else
            {
                // Item is link
                if (Context.Item.IsDerived(Templates.NavigationLink.ID))
                {
                    // Set item to target item
                    var targetItem = Context.Item.TargetItem(Templates.NavigationLink.Fields.Link);
                    Context.Item = targetItem;

                    // Set cache indicate item has been resolved
                    Context.Items[Constants.CustomUrlResloverCacheKey] = true;
                }
                else
                {
                    // Ignore if item is not Local Content
                    if (!args.Url.FilePath.ToLower().StartsWith(Constants.LocalContentPath)) return;

                    // Get routes for link provider
                    var routes = WildcardManager.Current?.GetWildcardRouteForLinkProvider(Context.Item, Context.Site);

                    // Resolve and block request on original item path
                    var result = WildcardItemResolver.Current?.ResolveItem(Context.Item, args.Url.FilePath, routes);

                    // Cannot found, no need to process
                    if (result == null)
                        return;

                    // Set item not found
                    Context.Item = null;

                    // Set cache indicate item has been resolved
                    Context.Items[Constants.CustomUrlResloverCacheKey] = true;
                }
            }   
        }

        /// <summary>
        /// Indicates whether the request should be ignored.
        /// </summary>
        /// <param name="args">The pipeline arguments.</param>
        /// <returns></returns>
        private bool IgnoreRequest(HttpRequestArgs args)
        {
            return Context.Site == null || Context.Domain == null || Context.Item == null || Context.Database == null
                || Configuration.IgnoreSites.Count != 0 && Configuration.IgnoreSites.Contains(Context.Site.Name.ToLower())
                || args.Url.ItemPath.Length == 0
                || Context.Domain.Name.Equals(Constants.Sitecore.DefaultDomain, StringComparison.InvariantCultureIgnoreCase) && Context.Site.DisplayMode != DisplayMode.Edit
                || Context.Database.Name.Equals(Constants.Sitecore.CoreDatabse, StringComparison.InvariantCultureIgnoreCase)
                || Context.Item.IsItemNotFound();
        }
    }
}