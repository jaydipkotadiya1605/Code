namespace Sitecore.Foundation.Search.Models.Index
{
    using Sitecore.ContentSearch;
    using Sitecore.Foundation.Indexing.Models;

    public class SiteIndex : IndexedItem
    {
        [IndexField("site_code_t")]
        public string SiteCode { get; set; }
    }
}