namespace Sitecore.Foundation.Search.Models.Index
{
    using System;
    using Sitecore.ContentSearch;
    using Sitecore.Foundation.Indexing.Models;

    public class BannerIndex : IndexedItem
    {
		[IndexField("post_date_tdt")]
        public DateTime PostDate { get; set; }

        [IndexField("expiry_date_tdt")]
        public DateTime ExpiryDate { get; set; }

        [IndexField("expiry_date_has_value_b")]
        public bool ExpiryDateHasValue { get; set; }

        [IndexField("post_date_has_value_b")]
        public bool PostDateHasValue { get; set; }
    }
}