using Sitecore.Configuration;
using Sitecore.Foundation.ItemResolver.Providers.ItemResolving;

namespace Sitecore.Foundation.ItemResolver.Providers
{
    /// <summary>
    /// Wildcard ItemResolver
    /// </summary>
    public static class WildcardItemResolver
    {
        private static readonly ProviderHelper<ItemResolving.ItemResolver, ItemResolverCollection> Configuration;

        /// <inheritdoc />
        static WildcardItemResolver()
        {
            Configuration = new ProviderHelper<ItemResolving.ItemResolver, ItemResolverCollection>(Constants.ConfigKey.WildcardItemResolver);
        }

        /// <summary>
        /// Current Provider
        /// </summary>
        public static ItemResolving.ItemResolver Current => Configuration?.Provider;

        /// <summary>
        /// All Providers
        /// </summary>
        public static ItemResolverCollection Providers => Configuration?.Providers;
    }
}