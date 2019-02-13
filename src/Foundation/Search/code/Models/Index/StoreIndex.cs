namespace Sitecore.Foundation.Search.Models.Index
{
    using System;
    using System.Collections.Generic;
    using Sitecore.ContentSearch;

    public class StoreIndex : PageViewsIndex
    {
        [IndexField("store_name")]
        public string StoreName { get; set; }

        [IndexField("phone_number")]
        public string PhoneNumber { get; set; }

        [IndexField("new_date")]
        public DateTime NewDate { get; set; }

        [IndexField("store_next_three_months_new_date")]
        public DateTime NextThreeMonthsNewDate { get; set; }

        [IndexField("post_date")]
        public DateTime PostDate { get; set; }

        [IndexField("store_expired_date")]
        public DateTime ExpiryDate { get; set; }

        [IndexField("store_has_upcoming_or_new_date")]
        public bool StoreHasUpcomingOrNewDate { get; set; }

        [IndexField("store_categories")]
        public IList<string> Categories { get; set; }

        [IndexField("description")]
        public string Description { get; set; }

        [IndexField("unit_no")]
        public string UnitNo { get; set; }

        [IndexField("contact")]
        public string Contact { get; set; }

        [IndexField("opening_hours")]
        public string OpeningHours { get; set; }

        [IndexField("store_offers")]
        public IList<string> StoreOffers { get; set; }

        [IndexField("brands")]
        public string Brands { get; set; }

        [IndexField("keywords")]
        public string Keywords { get; set; }

        [IndexField("wing")]
        public string Wing { get; set; }

        [IndexField("store_mall_site")]
        public string MallSite { get; set; }
    }
}