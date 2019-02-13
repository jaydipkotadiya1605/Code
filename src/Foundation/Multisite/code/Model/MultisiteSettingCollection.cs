namespace Sitecore.Foundation.Multisite.Model
{
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;

    public class MultisiteSettingCollection
    {
        public IEnumerable<RemoveItem> RemoveItemIds { get; set; }
        public IEnumerable<Item> CloneSites { get; set; }

        public MultisiteSettingCollection(NameValueCollection targetIds, Item[] allSite, Item[] siteSettings)
        {
            this.RemoveItemIds = this.GetRemoveItems(targetIds, allSite, siteSettings);
            this.CloneSites = this.GetCloneSites(targetIds, allSite, siteSettings);
        }

        private List<RemoveItem> GetRemoveItems(NameValueCollection targetIds, Item[] allSite, Item[] siteSettings)
        {
            var result = new List<RemoveItem>();
            foreach (var site in allSite)
            {
                var siteId = site.ID.ToShortID().ToString();
                if ((siteSettings == null || !siteSettings.Any(x => x.ID.Equals(site.ID))) && targetIds[siteId] != null)
                {
                    var deleteItemId = targetIds[siteId];
                    result.Add(new RemoveItem
                    {
                        DeleteItemId = new ID(deleteItemId),
                        SiteId = new ID(siteId)
                    });
                }
            }
            return result;
        }

        private List<Item> GetCloneSites(NameValueCollection targetIds, Item[] allSite, Item[] siteSettings)
        {
            var result = new List<Item>();
            foreach (var site in allSite)
            {
                if (siteSettings != null && siteSettings.Any(x => x.ID.Equals(site.ID)) && targetIds[site.ID.ToShortID().ToString()] == null)
                {
                    result.Add(site);
                }
            }
            return result;
        }
    }

    public class RemoveItem
    {
        public ID DeleteItemId { get; set; }
        public ID SiteId { get; set; }
    }
}