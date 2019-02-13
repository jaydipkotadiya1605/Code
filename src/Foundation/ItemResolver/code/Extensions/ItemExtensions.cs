using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Foundation.ItemResolver.Providers;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using System;
using System.Linq;

namespace Sitecore.Foundation.ItemResolver.Extensions
{
    public static class ItemExtensions
    {
        public static bool IsWilcard([NotNull] this Item item)
        {
            return item.TemplateID == Templates.Wildcard.ID && item.Name.StartsWith(Constants.Wildcard.Node);
        }

        public static bool IsItemNotFound([NotNull] this Item item)
        {
            return item.TemplateID == Templates.ItemNotFoundPage.ID;
        }

        public static Item GetDroptreeItem([NotNull] this Item item, ID field)
        {
            return item != null && !field.IsNull && item.Fields[field].HasValue ? item.Database.GetItem(item.Fields[field].Value.ItemId()) : null;
        }

        public static Item GetParent([NotNull] this Item item)
        {
            // Get routes for item
            var routes = WildcardManager.Current?.GetWildcardRouteForLinkProvider(item, Context.Site);
            if (routes == null)
            {
                // Return parent of item
                return item.Parent;
            }

            foreach (var route in routes)
            {
                // Route is existed and request need to be processed
                if (route != null && route.IsValid && route.DataTemplate.ID == item.TemplateID)
                {
                    // Return parent of wildcard
                    return route.WildcardNode.Parent;
                }
            }

            // Return parent of item
            return item.Parent;
        }

        public static Item GetContext([NotNull] this Item item)
        {
            // Get route for item
            var routes = WildcardManager.Current?.GetWildcardRouteForLinkProvider(item, Context.Site);
            if (routes == null)
            {
                // Return item
                return item;
            }

            foreach (var route in routes)
            {
                // Route is existed and request need to be processed
                if (route != null && route.IsValid && route.DataTemplate.ID == item.TemplateID)
                {
                    // Return wildcard
                    return route.WildcardNode;
                }
            }

            // Return item
            return item;
        }

        public static string SiteName([NotNull] this Item item)
        {
            var site = item?.GetAncestorOrSelfOfTemplate(FrasersContent.Templates.Identity.ID);
            return site?.Fields[FrasersContent.Templates.Identity.Fields.SiteName]?.Value.Replace(Constants.Blank, string.Empty) ?? string.Empty;
        }

        public static bool HasWildCard([NotNull] this Item item)
        {
            var itemWilcard = item.HasChildren ? item.Children.First() : null;
            return itemWilcard != null && itemWilcard.IsWilcard();
        }

        public static bool IsOnCurrentSite([NotNull] this Item item)
        {
            var site = item?.GetAncestorOrSelfOfTemplate(FrasersContent.Templates.Identity.ID);
            if (Context.Site == null || site == null)
            {
                return false;
            }

            return Context.Site.IsMainSite() || Context.Site.IsMallSite() && site.Name.Equals(Context.Site.Name, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}