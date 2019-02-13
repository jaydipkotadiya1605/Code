namespace Sitecore.Foundation.Multisite.Extensions
{
    using Sitecore.Data.Fields;
    using Sitecore.Data.Items;
    using Sitecore.Foundation.SitecoreExtensions.Extensions;
    using Sitecore.Buckets.Extensions;
    using Sitecore.Buckets.Managers;

    public static class MultiSiteItemExtensions
    {
        public static string GetSitePath(this Item item)
        {
            var paths = item.Paths.Path.Split('/');
            if (paths.Length >= 4)
            {
                return $"{paths[0]}/{paths[1]}/{paths[2]}/{paths[3]}";
            }
            return string.Empty;
        }
        public static Item GetSiteItem(this Item item)
        {
            var master = item.Database;
            var path = item.GetSitePath();
            return master.GetItem(path);

        }
        public static Item GetMainSite(this Item item)
        {
            if (item.IsBelongToMainSite())
            {
                return item.GetSiteItem();
            }
            if (item.IsBelongToMallSite())
            {
                var site = item.GetSiteItem();
                return site.TargetItem(Templates.MallSiteSetting.Fields.MainSite);
            }
            return null;
        }
        public static Item[] GetMallSites(this Item mainSite)
        {
            if (mainSite.IsDerived(Templates.MainSiteSetting.ID))
            {
                return ((MultilistField)mainSite.Fields[Templates.MainSiteSetting.Fields.MallSites]).GetItems();
            }
            return new Item[0];
        }
        public static bool IsBelongToMainSite(this Item item)
        {
            var site = item.GetSiteItem();
            return site.IsDerived(Templates.MainSiteSetting.ID);
        }

        public static bool IsBelongToMallSite(this Item item)
        {
            var site = item.GetSiteItem();
            return site.IsDerived(Templates.MallSiteSetting.ID);
        }
        public static Item GetDestinationItem(this Item sourceItem, Item destinationSite)
        {
            var parentSourceItem = sourceItem.Parent;
            var saveParentItemPath = BucketManager.IsItemContainedWithinBucket(sourceItem) 
                ? parentSourceItem.GetParentBucketItemOrParent().Paths.Path 
                : parentSourceItem.Paths.Path;
            var destinationPath = saveParentItemPath.Replace(sourceItem.GetSitePath(), destinationSite.GetSitePath());
            return destinationSite.Database.GetItem(destinationPath);
        }
    }
}