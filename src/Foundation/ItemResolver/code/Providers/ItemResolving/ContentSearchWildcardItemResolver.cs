using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Foundation.ItemResolver.Extensions;
using Sitecore.Foundation.ItemResolver.Providers.RouteResolving;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Sitecore.Foundation.ItemResolver.Providers.ItemResolving
{
    /// <summary>
    /// ContentSearch Wildcard ItemResolver
    /// </summary>
    public class ContentSearchWildcardItemResolver : ItemResolver
    {
        private static readonly ConcurrentDictionary<string, Item> ItemResolverResultCache = new ConcurrentDictionary<string, Item>();

        /// <summary>
        /// Search Item from index
        /// </summary>
        /// <param name="contextItem">Context item</param>
        /// <param name="itemName">Item name</param>
        /// <param name="templateId">Item teamplate Id</param>
        /// <param name="searchRootNode">Search root node</param>
        /// <param name="includeSiteName">Include site name</param>
        /// <returns></returns>
        private Item SearchItemFromIndex(Item contextItem, string itemName, ID templateId, ID searchRootNode, bool includeSiteName)
        {
            string siteName = string.Empty;
            // Seperate siteName from itemName
            if (includeSiteName)
            {
                siteName = itemName?.Split(new[] { Constants.Blank }, StringSplitOptions.RemoveEmptyEntries)?.Last() ?? string.Empty;
                itemName = itemName?.Replace(siteName, string.Empty);
            }
            // try to search and resolve item from index
            var indexable = new SitecoreIndexableItem(contextItem);
            using (var searchContext = ContentSearchManager.GetIndex(indexable).CreateSearchContext())
            {
                var results = searchContext.GetQueryable<SearchResultItem>()
                    .Where(x => x.TemplateId == templateId
                                && x.Paths.Contains(searchRootNode)
                                && x.Language.Equals(Context.Language.Name, StringComparison.InvariantCultureIgnoreCase)
                                && x.Name.Equals(itemName, StringComparison.InvariantCultureIgnoreCase))
                    .ToList();

                if (!results.Any())
                {
                    return null;
                }

                return includeSiteName && !string.IsNullOrEmpty(siteName)
                    ? results.First(x => x.GetItem().SiteName().NormalizeItemName() == siteName)?.GetItem() ?? null
                    : results.First()?.GetItem() ?? null;
            }
        }

        /// <summary>
        /// Resolve Item
        /// </summary>
        /// <param name="contextItem">Context item</param>
        /// <param name="itemName">Item name</param>
        /// <param name="templateId">Item teamplate Id</param>
        /// <param name="searchRootNode">Search root node</param>
        /// <returns></returns>
        public virtual Item ResolveItem(Item contextItem, string itemName, ID templateId, ID searchRootNode, bool includeSiteName)
        {
            // Try to search from index
            var itemFromIndex = this.SearchItemFromIndex(contextItem, itemName, templateId, searchRootNode, includeSiteName);
            return itemFromIndex;
        }


        /// <summary>
        /// Resolve Item
        /// </summary>
        /// <param name="contextItem">Context Item</param>
        /// <param name="url">Request Url</param>
        /// <param name="routeList">Route List</param>
        /// <returns></returns>
        public override Item ResolveItem(Item contextItem, string url, List<WildcardRouteItem> routeList)
        {
            if (contextItem == null || routeList == null || routeList.Count == 0)
            {
                return null;
            }

            // Get item name from Url
            var name = url.ItemName();

            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            var cacheKey = $"cache{contextItem.ID}_{name}_{contextItem.Language.Name}_{Context.Site.Name}";
            if (ItemResolverResultCache.ContainsKey(cacheKey))
            {
                return ItemResolverResultCache[cacheKey];
            }

            foreach (var route in routeList)
            {
                var result = this.ResolveItem(contextItem, name, route.DataTemplate.ID, route.SearchRootNode.ID, route.IncludeSiteName);
                if(result != null)
                {
                    ItemResolverResultCache.TryAdd(cacheKey, result);
                    return result;
                }
            }

            return null;
        }

        /// <inheritdoc />
        public override void ClearCache() => ItemResolverResultCache.Clear();
    }
}