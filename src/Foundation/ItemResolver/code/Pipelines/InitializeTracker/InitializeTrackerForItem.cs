using Sitecore.Analytics.Pipelines.InitializeTracker;
using Sitecore.Analytics.Tracking;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Foundation.ItemResolver.Extensions;
using Sitecore.Foundation.ItemResolver.Providers;
using Sitecore.Web;
using System;
using System.Web;

namespace Sitecore.Foundation.ItemResolver.Pipelines.InitializeTracker
{
    /// <summary>
    /// Process InitializeTrackerProcessor pipeline
    /// </summary>
    public class InitializeTrackerForItem : InitializeTrackerProcessor
    {
        /// <summary>
        /// Processes the specified HttpRequest pipeline arguments.
        /// </summary>
        /// <param name="args">The HttpRequest arguments.</param>
        public override void Process(InitializeTrackerArgs args)
        {
            Assert.ArgumentNotNull(args, nameof(args));

            // Ignore requests if itemresolver should not be applied (see method for details).
            if (this.IgnoreRequest(args))
            {
                return;
            }

            var httpContext = args.HttpContext;
            if (httpContext == null)
            {
                args.AbortPipeline();
                return;
            }


            this.CreateAndInitializePage(httpContext, args.Session.Interaction);
        }

        private Item ResolveWildcardItem(HttpContextBase httpContext, Item item)
        {
            // Get routes for item resolver
            var routes = WildcardManager.Current?.GetWildcardRouteForItemResolver(item, Context.Site);

            // Reslove item by url and routes
            return WildcardItemResolver.Current?.ResolveItem(item, httpContext.Request.FilePath, routes);
        }

        private void CreateAndInitializePage(HttpContextBase httpContext, CurrentInteraction visit)
        {
            var empty = visit.CreatePage();
            empty.SetUrl(WebUtil.GetRawUrl());

            var device = Context.Device;
            if (device == null)
            {
                empty.SitecoreDevice.Id = Guid.Empty;
                empty.SitecoreDevice.Name = string.Empty;
            }
            else
            {
                empty.SitecoreDevice.Id = device.ID.Guid;
                empty.SitecoreDevice.Name = device.Name;
            }

            // Default Sitecore implementation
            var item = Context.Item;

            // If the current item is a wildcard
            if (item != null && item.IsWilcard())
            {
                // Perform a call to the logic which resolves the correct item
                var resolvedItem = this.ResolveWildcardItem(httpContext, item);

                if (resolvedItem != null)
                {
                    item = resolvedItem;
                }
            }

            // Resume the default behaviour
            if (item == null)
            {
                return;
            }

            empty.SetItemProperties(item.ID.Guid, item.Language.Name, item.Version.Number);
        }

        /// <summary>
        /// Indicates whether the request should be ignored.
        /// </summary>
        /// <param name="args">The pipeline arguments.</param>
        /// <returns></returns>
        private bool IgnoreRequest(InitializeTrackerArgs args)
        {
            return Context.Site == null || Context.Item == null || Context.Database == null
                || (Configuration.IgnoreSites.Count != 0 && Configuration.IgnoreSites.Contains(Context.Site.Name.ToLower()))
                || args.IsSessionEnd;
        }
    }
}