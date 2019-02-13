namespace Sitecore.Feature.Store.Models
{
    using System;
    using System.Collections.Generic;
    using Sitecore.Foundation.SitecoreExtensions.Models;

    public class StoreViewModel
    {
        public string Id { get; set; }
        public  string Url { get; set; }
        public string StoreName { get; set; }
        public string Description { get; set; }
        public ImageItem Logo { get; set; }
        public string PhoneNumber { get; set; }
        public string Contact { get; set; }
        public string OpeningHours { get; set; }
        public string UnitNo { get; set; }
        public string Brands { get; set; }
        public string Keywords { get; set; }
        public string Mall { get; set; }
        public DateTime NewDate { get; set; }
        public DateTime PostDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public IEnumerable<StoreCategoryViewModel> Categories { get; set; }
        public IEnumerable<StoreOfferViewModel> Offers { get; set; }
        public StoreStatus Status { get; set; }
    }
}