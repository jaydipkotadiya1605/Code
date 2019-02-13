using Sitecore.Data.Items;
using Sitecore.Sites;

namespace Sitecore.Foundation.ItemResolver.Extensions
{
    using Sitecore.Foundation.SitecoreExtensions.Extensions;

    public static class SiteContextExtensions
    {
        private static Item Settings(this SiteContext site, string settings)
        {
            var path = site?.Properties[settings];
            return path != null && site.Database != null ? site.Database.GetItem(path) : null;
        }

        public static Item ItemResolverSettings(this SiteContext site)
        {
            return Settings(site, Constants.ConfigKey.ItemResolverSettingsConfig);
        }

        public static bool IsMainSite(this SiteContext site)
        {
            var siteRoot = Context.Database.GetItem(site.RootPath);
            return siteRoot.IsDerived(Templates.MainSite.ID);
        }

        public static bool IsMallSite(this SiteContext site)
        {
            var siteRoot = Context.Database.GetItem(site.RootPath);
            return siteRoot.IsDerived(Templates.MallSite.ID);
        }
    }
}