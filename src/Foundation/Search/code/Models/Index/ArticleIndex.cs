namespace Sitecore.Foundation.Search.Models.Index
{
    using System;
    using Sitecore.ContentSearch;
    using Sitecore.Foundation.Indexing.Models;

    public class ArticleIndex : IndexedItem
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

        [IndexField("article_category_s")]
        public string ArticleCategory { get; set; }

        [IndexField("expiry_date_has_value_b")]
        public bool ExpiryDateHasValue { get; set; }

        [IndexField("post_date_has_value_b")]
        public bool PostDateHasValue { get; set; }

        [IndexField("related_store_s")]
        public string Store { get; set; }

        [IndexField("store_mall_site")]
        public string MallSite { get; set; }

    }
}