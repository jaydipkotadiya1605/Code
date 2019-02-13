namespace Sitecore.Feature.Search.ViewModels
{
    using Sitecore.Foundation.Indexing.Models;
    using System.Collections.Generic;
    using Sitecore.Foundation.Search.Models;

    public class FraserRewardsViewModel : PagingSettings
    {
        public string RenderingId { get; set; }
        public string Title { get; set; }
        public string NotFoundMessage { get; set; }
        public int TotalNumberOfResults { get; set; }
        public bool IsHiddenIfNotFound { get; set; }
        public bool HasPaging => this.TotalNumberOfResults > (this.PageIndex * this.PageSize);
        public IEnumerable<ISearchResult> Results { get; set; }
    }
}