namespace Sitecore.Feature.Store.Models
{
    using Sitecore.Data;
    using Sitecore.Foundation.Search.Models;

    public class RelatedStoresSearchCriteria : PagingSettings
    {
        public ID CurrentMallId { get; set; }
        public ID CurrentStoreId { get; set; }
    }
}