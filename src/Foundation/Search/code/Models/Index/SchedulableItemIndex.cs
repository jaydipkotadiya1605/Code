namespace Sitecore.Foundation.Search.Models.Index
{
    using System;
    using Sitecore.ContentSearch;
    using Sitecore.Foundation.Indexing.Models;

    public class SchedulableItemIndex : IndexedItem
    {
        [IndexField("post_date")]
        public DateTime PostDate { get; set; }

        [IndexField("post_date_has_value")]
        public bool PostDateHasValue { get; set; }

        [IndexField("expiry_date")]
        public DateTime ExpiryDate { get; set; }

        [IndexField("expiry_date_has_value")]
        public bool ExpiryDateHasValue { get; set; }
    }
}