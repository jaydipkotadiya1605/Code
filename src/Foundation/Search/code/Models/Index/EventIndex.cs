namespace Sitecore.Foundation.Search.Models.Index
{
    using System;
    using Sitecore.ContentSearch;
    using Sitecore.Foundation.Indexing.Models;
    using System.Collections.Generic;

    public class EventIndex : IndexedItem
    {
        [IndexField("title_s")]
        public string Title { get; set; }

        [IndexField("start_date_tdt")]
        public DateTime StartDate { get; set; }

        [IndexField("end_date_tdt")]
        public DateTime EndDate { get; set; }
		
		[IndexField("post_date_tdt")]
        public DateTime PostDate { get; set; }

        [IndexField("expiry_date_tdt")]
        public DateTime ExpiryDate { get; set; }

        [IndexField("event_category_s")]
        public string Category { get; set; }

        [IndexField("is_special_event_b")]
        public bool IsSpecialEvent { get; set; }

        [IndexField("event_show_in_mall_sm")]
        public IList<string> Malls { get; set; }

        [IndexField("event_mall_name_s")]
        public string MallName { get; set; }

        [IndexField("expiry_date_has_value_b")]
        public bool ExpiryDateHasValue { get; set; }

        [IndexField("post_date_has_value_b")]
        public bool PostDateHasValue { get; set; }
    }
}