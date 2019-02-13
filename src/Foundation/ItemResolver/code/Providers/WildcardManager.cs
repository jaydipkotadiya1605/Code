using Sitecore.Configuration;
using Sitecore.Foundation.ItemResolver.Providers.RouteResolving;

namespace Sitecore.Foundation.ItemResolver.Providers
{
    /// <summary>
    /// Wildcard Manager
    /// </summary>
    public static class WildcardManager
    {
        private static readonly ProviderHelper<RouteResolver, RouteResolverCollection> Configuration;

        /// <inheritdoc />
        static WildcardManager()
        {
            Configuration = new ProviderHelper<RouteResolver, RouteResolverCollection>(Constants.ConfigKey.WildcardManager);
        }

        /// <summary>
        /// Current Provider
        /// </summary>
        public static RouteResolver Current => Configuration?.Provider;

        /// <summary>
        /// All Providers
        /// </summary>
        public static RouteResolverCollection Providers => Configuration?.Providers;
    }
}