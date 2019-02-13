namespace Sitecore.Foundation.Search.Repositories
{
    using Sitecore.ContentSearch.Linq;
    using Sitecore.Foundation.Abstractions.SitecoreContext;
    using Sitecore.Foundation.Indexing.Models;
    using Sitecore.Foundation.Search.Models;
    using Sitecore.Foundation.SitecoreExtensions.Extensions;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class SearchRepository : ISearchRepository
    {
        protected const float DefaultBoost = 100f;
        protected const float DefaultSimilarity = 0.8f;
        protected ISitecoreContext _sitecoreContext;

        private readonly string IndexPattern = "sitecore_{0}_{1}_index";

        protected string IndexName => string.Format(this.IndexPattern, this._sitecoreContext.SiteName, this._sitecoreContext.Database?.Name);

        protected SearchRepository(ISitecoreContext sitecoreContext)
        {
            this._sitecoreContext = sitecoreContext;
        }

        protected virtual IList<ScoredItem> BuildResultModel<T>(SearchResults<T> results) where T : IndexedItem
        {
            return results.Hits.Select(this.BuildScoredItemModel).Where(x => x != null).ToList();
        }

        protected virtual ScoredItem BuildScoredItemModel<T>(SearchHit<T> searchItem) where T: IndexedItem
        {
            var item = searchItem.Document.GetItem();
            if (item == null) return null;

            return new ScoredItem
            {
                Item = item,
                Score = searchItem.Score,
                ItemUrl = item.Url()
            };
        }
    }
}