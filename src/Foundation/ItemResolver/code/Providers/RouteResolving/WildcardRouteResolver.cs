using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Foundation.ItemResolver.Extensions;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Sites;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Sitecore.Foundation.ItemResolver.Providers.RouteResolving
{
    /// <inheritdoc />
    /// <summary>
    /// Wildcard Route resolver provider
    /// </summary>
    public class WildcardRouteResolver : RouteResolver
    {
        private static readonly ConcurrentDictionary<string, List<WildcardRouteItem>> RoutesCache = new ConcurrentDictionary<string, List<WildcardRouteItem>>();
        private static readonly ConcurrentDictionary<string, List<WildcardRouteItem>> ItemResolverRoutesCache = new ConcurrentDictionary<string, List<WildcardRouteItem>>();
        private static readonly ConcurrentDictionary<string, List<WildcardRouteItem>> LinkProviderRoutesCache = new ConcurrentDictionary<string, List<WildcardRouteItem>>();
        
        /// <summary>
        /// Get routes for site
        /// </summary>
        /// <param name="site">Site context</param>
        /// <returns></returns>
        private List<WildcardRouteItem> GetRoutesForSite(SiteContext site)
        {
            Assert.ArgumentNotNull(site, nameof(site));

            if (Context.Database == null || site == null)
            {
                return new List<WildcardRouteItem>();
            }

            var routeKey = $"route_{Context.Language.Name.ToLower()}_{site.Name.ToLower()}";
            if (RoutesCache.ContainsKey(routeKey))
            {
                return RoutesCache[routeKey];
            }
          
            // get Item Resolver Settings
            var itemResolverSettings = site.ItemResolverSettings();

            if (itemResolverSettings == null || !itemResolverSettings.IsDerived(Templates.ItemResolverSettings.ID))
            {
                return new List<WildcardRouteItem>();
            }

            var routes = itemResolverSettings.Children.Select(x => new WildcardRouteItem(x)).ToList();
            if (routes.Any())
            {
                RoutesCache.TryAdd(site.Name.ToLower(), routes);
            }

            return routes;
        }
        /// <inheritdoc />
        /// <summary>
        /// Get widlcard routes for item resolver
        /// </summary>
        /// <param name="item">Context item</param>
        /// <param name="site">Context site</param>
        /// <returns></returns>
        public override List<WildcardRouteItem> GetWildcardRouteForItemResolver(Item item, SiteContext site)
        {
            Assert.ArgumentNotNull(item, nameof(item));
            Assert.ArgumentNotNull(site, nameof(site));

            var cacheKey = $"cache{item.ID}_{item.Language.Name}_{site.Name}";
            if (ItemResolverRoutesCache.ContainsKey(cacheKey))
            {
                return ItemResolverRoutesCache[cacheKey];
            }

            var routes = this.GetRoutesForSite(site);
            List<WildcardRouteItem> wildcardRoutes = routes.Where(x => x.WildcardNode != null && x.WildcardNode.ID == item.ID).ToList();
            if (wildcardRoutes.Any())
            {
                ItemResolverRoutesCache.TryAdd(cacheKey, wildcardRoutes);
            }

            return wildcardRoutes;
        }

        /// <inheritdoc />
        /// <summary>
        /// Get widlcard routes for link provider
        /// </summary>
        /// <param name="item">Context item</param>
        /// <param name="site">Context site</param>
        /// <returns></returns>
        public override List<WildcardRouteItem> GetWildcardRouteForLinkProvider(Item item, SiteContext site)
        {
            Assert.ArgumentNotNull(item, nameof(item));
            Assert.ArgumentNotNull(site, nameof(site));

            var cacheKey = $"cache{item.TemplateID}_{item.Language.Name}_{site.Name}";
            if (LinkProviderRoutesCache.ContainsKey(cacheKey))
            {
                return LinkProviderRoutesCache[cacheKey];
            }

            var routes = this.GetRoutesForSite(site);
            List<WildcardRouteItem> wildcardRoutes = routes.Where(x => x.DataTemplate != null && x.DataTemplate.ID == item.TemplateID).ToList();
            if (wildcardRoutes.Any())
            {
                LinkProviderRoutesCache.TryAdd(cacheKey, wildcardRoutes);
            }

            return wildcardRoutes;
        }

        /// <inheritdoc />
        /// <summary>
        /// Clear cache
        /// </summary>
        public override void ClearCache()
        {
            RoutesCache.Clear();
            ItemResolverRoutesCache.Clear();
            LinkProviderRoutesCache.Clear();
        }
    }
}