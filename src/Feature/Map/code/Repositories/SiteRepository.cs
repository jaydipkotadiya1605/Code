using Sitecore.Feature.Map.Models;
using Sitecore.Foundation.Abstractions.SitecoreContext;
using Sitecore.Foundation.DependencyInjection;
using Sitecore.Foundation.SitecoreExtensions.Extensions;

namespace Sitecore.Feature.Map.Repositories
{
    [Service(typeof(ISiteRepository))]
    public class SiteRepository : ISiteRepository
    {
        private readonly ISitecoreContext sitecoreContext;

        public SiteRepository(ISitecoreContext sitecoreContext)
        {
            this.sitecoreContext = sitecoreContext;
        }
        public ContactInfo GetShopInfo()
        {
            var siteItem = this.sitecoreContext.Database.GetItem(Context.Site.StartPath)?.Parent;
            if (siteItem == null || !siteItem.IsDerived(Templates.Contact.ID))
            {
                return null;
            }

            return new ContactInfo()
            {
                CallUs = siteItem.Field(Templates.Contact.Fields.Tel),
                CustomerServiceCounter = siteItem.Field(Templates.Contact.Fields.CustomerService),
                FindUs = siteItem.Field(Templates.Contact.Fields.Address),
                OpenFrom = siteItem.Field(Templates.Contact.Fields.WorkingHours)
            };
        }

        public MapLocation GetMapLocation()
        {
            var siteItem = this.sitecoreContext.Database.GetItem(Context.Site.StartPath)?.Parent;
            var map = new MapLocation();
            if (siteItem == null || !siteItem.IsDerived(Templates.Contact.ID))
            {
                return null;
            }

            map.Longitude = siteItem.GetString(Templates.Contact.Fields.Longitude);
            map.Lattitude = siteItem.GetString(Templates.Contact.Fields.Lattitude);

            if (!siteItem.IsDerived(Templates.SiteMetadata.ID))
                return string.IsNullOrEmpty(map.Longitude) || string.IsNullOrEmpty(map.Lattitude) || string.IsNullOrEmpty(map.ShopName) ? null : map;

            map.ShopName = siteItem.GetString(Templates.SiteMetadata.Fields.BrowserTitle);

            return string.IsNullOrEmpty(map.Longitude) || string.IsNullOrEmpty(map.Lattitude) || string.IsNullOrEmpty(map.ShopName) ? null : map;
        }
    }
}