using Sitecore.ContentSearch;
using Sitecore.Foundation.Indexing.Models;

namespace Sitecore.Foundation.Search.Models.Index
{
    public class PageViewsIndex : IndexedItem
    {
        [IndexField("page_views_tl")]
        public int PageViews { get; set; }
    }
}