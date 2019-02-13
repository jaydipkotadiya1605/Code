using Sitecore.Data.Items;
using Sitecore.Foundation.ItemResolver.Extensions;
using Sitecore.Foundation.SitecoreExtensions.Extensions;

namespace Sitecore.Foundation.ItemResolver.Providers.RouteResolving
{
    /// <summary>
    /// Wildcard Route Item
    /// </summary>
    public class WildcardRouteItem
    {
        /// <summary>
        /// Initialize Wildcard Route Item
        /// </summary>
        /// <param name="wildcardRouteItem">value to initialize</param>
        public WildcardRouteItem(Item wildcardRouteItem)
        {
            this.RouteItem = wildcardRouteItem;
        }

        /// <summary>
        /// Wildcard Node
        /// </summary>
        public Item WildcardNode => this.RouteItem?.GetDroptreeItem(Templates.ItemResolverRule.Fields.WildcardNode);

        /// <summary>
        /// Data Template
        /// </summary>
        public Item DataTemplate => this.RouteItem?.GetDroptreeItem(Templates.ItemResolverRule.Fields.DataTemplate);

        /// <summary>
        /// Search Root Node
        /// </summary>
        public Item SearchRootNode => this.RouteItem?.GetDroptreeItem(Templates.ItemResolverRule.Fields.SearchRootNode);

        /// <summary>
        /// Include Site Name
        /// </summary>
        public bool IncludeSiteName => this.RouteItem?.GetBoolFieldValue(Templates.ItemResolverRule.Fields.IncludeSiteName) ?? false;

        public bool IsValid => this.RouteItem != null && this.WildcardNode != null && this.SearchRootNode != null && this.DataTemplate != null;

        private Item RouteItem { get; }
    }
}