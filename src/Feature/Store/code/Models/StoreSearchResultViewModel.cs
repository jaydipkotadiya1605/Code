namespace Sitecore.Feature.Store.Models
{
    using Sitecore.Foundation.Search.Models;
    using System.Collections.Generic;

    public class StoreSearchResultViewModel : PagingSettings
    {
        public string RenderingId { get; set; }
        public string Keyword { get; set; }
        public IEnumerable<StoreViewModel> Stores { get; set; }
        public int TotalNumberOfResults { get; set; }
        public bool HasMoreResult { get; set; }
        public bool IsHiddenIfNotFound { get; set; }
    }
}