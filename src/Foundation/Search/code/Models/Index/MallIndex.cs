namespace Sitecore.Foundation.Search.Models.Index
{
    using Sitecore.ContentSearch;
    using Sitecore.Foundation.Indexing.Models;

    public class MallIndex : IndexedItem
    {
        [IndexField("main_site_s")]
        public string MainSite { get; set; }
    }
}