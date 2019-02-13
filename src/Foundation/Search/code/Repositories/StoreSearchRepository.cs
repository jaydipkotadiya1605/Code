namespace Sitecore.Foundation.Search.Repositories
{
    using System;
    using Sitecore.ContentSearch;
    using Sitecore.ContentSearch.Linq;
    using Sitecore.Foundation.Abstractions.SitecoreContext;
    using Sitecore.Foundation.DependencyInjection;
    using Sitecore.Foundation.Search.Models;
    using Sitecore.Foundation.Search.Models.Index;
    using System.Linq;
    using System.Linq.Expressions;
    using Sitecore.ContentSearch.Utilities;
    using Sitecore.Foundation.FrasersContent;

    [Service(typeof(IStoreSearchRepository))]
    public class StoreSearchRepository : SearchRepository, IStoreSearchRepository
    {
        public StoreSearchRepository(ISitecoreContext sitecoreContext) : base(sitecoreContext)
        {
        }

        public SearchItems Search(
            Expression<Func<StoreIndex, bool>> query,
            int pageIndex, int pageSize,
            Func<IQueryable<StoreIndex>, IQueryable<StoreIndex>> orderFunc = null)
        {
            using (var searchContext = ContentSearchManager.GetIndex(this.IndexName).CreateSearchContext())
            {
                var queryable = searchContext.GetQueryable<StoreIndex>()
                    .Where(query)
                    .Where(x => x.AllTemplates.Contains(IdHelper.NormalizeGuid(Templates.Store.ID)))
                    .Page(pageIndex, pageSize)
                    .Filter(x => x.IsLatestVersion && x.Language == this._sitecoreContext.Language.Name);

                queryable = orderFunc != null ? orderFunc(queryable) : queryable.OrderBy(x => x.StoreName);

                var results = queryable.GetResults();
                var items = this.BuildResultModel(results);
                var hits = results.TotalSearchResults;
                return new SearchItems(items, hits);
            }
        }
    }
}
