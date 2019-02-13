namespace Sitecore.Feature.Store.Models
{
    using System;
    using Sitecore.Foundation.Search.Models;
    using System.Collections.Generic;
    using System.Web;

    public class SearchCriteria : PagingSettings
    {
        public string CategoryId { get; set; }
        public string WingId { get; set; }
        public string AlphabetId { get; set; }
        public string MallId { get; set; }
        public string StoreOfferId { get; set; }
        public string Keyword { get; set; }
        public string RenderingId { get; set; }
        public string Hidden { get; set; }
        public bool IsHiddenIfNotFound => MainUtil.GetBool(this.Hidden, false);
        public IEnumerable<string> Offers => HttpUtility.UrlDecode(this.StoreOfferId ?? string.Empty).Split(new []{','}, StringSplitOptions.RemoveEmptyEntries);
    }
}