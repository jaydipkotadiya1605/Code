using Sitecore.Data.Items;
using Sitecore.Foundation.ItemResolver.Providers.RouteResolving;
using System;
using System.Collections.Generic;
using System.Configuration.Provider;

namespace Sitecore.Foundation.ItemResolver.Providers.ItemResolving
{
    /// <inheritdoc />
    /// <summary>
    /// Item resolver provider
    /// </summary>
    public class ItemResolver : ProviderBase
    {
        /// <summary>
        /// Resolve Item
        /// </summary>
        /// <param name="contextItem">Context Item</param>
        /// <param name="url">Request Url</param>
        /// <param name="routeList">Route List</param>
        /// <returns></returns>
        public virtual Item ResolveItem(Item contextItem, string url, List<WildcardRouteItem> routeList)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Clear cache
        /// </summary>
        public virtual void ClearCache()
        {
            throw new NotImplementedException();
        }
    }
}