namespace Sitecore.Foundation.Search.Repositories
{
    using Sitecore.ContentSearch;
    using Sitecore.ContentSearch.Linq;
    using Sitecore.Foundation.Abstractions.SitecoreContext;
    using Sitecore.Foundation.DependencyInjection;
    using Sitecore.Foundation.Indexing.Models;
    using Sitecore.Foundation.Search.Models;
    using Sitecore.Foundation.Search.Models.Index;
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Sitecore.Foundation.Indexing.Repositories;

    [Service(typeof(IGlobalSearchRepository))]
    public class GlobalSearchRepository : SearchRepository, IGlobalSearchRepository
    {
        private SearchResultsFactory SearchResultsFactory { get;}

        public GlobalSearchRepository(ISitecoreContext sitecoreContext, SearchResultsFactory searchResultsFactory) : base(sitecoreContext)
        {
            this.SearchResultsFactory = searchResultsFactory;
        }

        public SearchItems Search<T>(Expression<Func<T, bool>> query, int pageIndex = 0, int pageSize = 0) where T : IndexedItem
        {
            using (var searchContext = ContentSearchManager.GetIndex(this.IndexName).CreateSearchContext())
            {
                var results = this.ExecuteQuery(searchContext, query, null, pageIndex, pageSize, null);
                var items = this.BuildResultModel(results);
                var hits = results.TotalSearchResults;
                return new SearchItems(items, hits);
            }
        }

        public ISearchResults Search<T>(
            Expression<Func<T, bool>> query,
            Expression<Func<T, bool>> filter,
            int pageIndex,
            int pageSize,
            Func<IQueryable<T>, IQueryable<T>> orderFunc) where T : FraserRewardsIndex
        {
            using (var searchContext = ContentSearchManager.GetIndex(this.IndexName).CreateSearchContext())
            {
                var result = this.ExecuteQuery(searchContext, query, filter, pageIndex, pageSize, orderFunc);
                return this.SearchResultsFactory.Create(result, null);
            }
        }

        private SearchResults<T> ExecuteQuery<T>(
            IProviderSearchContext searchContext,
            Expression<Func<T, bool>> query,
            Expression<Func<T, bool>> filter,
            int pageIndex,
            int pageSize,
            Func<IQueryable<T>, IQueryable<T>> orderFunc) where T : IndexedItem
        {
            var queryable = searchContext.GetQueryable<T>()
                .Where(query)
                .Filter(x => x.IsLatestVersion && x.Language == this._sitecoreContext.Language.Name);
            if (pageSize > 0)
            {
                queryable = queryable.Page(pageIndex, pageSize);
            }
            queryable = filter != null ? queryable.Filter(filter) : queryable;
            queryable = orderFunc != null ? orderFunc(queryable) : queryable.OrderBy(x => x.Name);
            return queryable.GetResults();
        }
    }
}