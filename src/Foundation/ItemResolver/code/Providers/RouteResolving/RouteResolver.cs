using Sitecore.Data.Items;
using Sitecore.Sites;
using System;
using System.Collections.Generic;
using System.Configuration.Provider;

namespace Sitecore.Foundation.ItemResolver.Providers.RouteResolving
{
    /// <inheritdoc />
    /// <summary>
    /// Route resolver provider
    /// </summary>
    public class RouteResolver : ProviderBase
    {
        /// <summary>
        /// Get widlcard routes for item resolver
        /// </summary>
        /// <param name="item">Context item</param>
        /// <param name="site">Context site</param>
        /// <returns></returns>
        public virtual List<WildcardRouteItem> GetWildcardRouteForItemResolver(Item item, SiteContext site)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get widlcard routes for link provider
        /// </summary>
        /// <param name="item">Context item</param>
        /// <param name="site">Context site</param>
        /// <returns></returns>
        public virtual List<WildcardRouteItem> GetWildcardRouteForLinkProvider(Item item, SiteContext site)
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