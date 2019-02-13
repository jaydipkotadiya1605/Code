namespace Sitecore.Foundation.Search.Repositories
{
    using Sitecore.Foundation.Indexing.Models;
    using Sitecore.Foundation.Search.Models;
    using Sitecore.Foundation.Search.Models.Index;
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public interface IGlobalSearchRepository : ISearchRepository
    {
        SearchItems Search<T>(Expression<Func<T, bool>> query, int pageIndex = 0, int pageSize = 0) where T : IndexedItem;

        ISearchResults Search<T>(Expression<Func<T, bool>> query, Expression<Func<T, bool>> filter, int pageIndex, int pageSize, Func<IQueryable<T>, IQueryable<T>> orderFunc) where T : FraserRewardsIndex;
    }
}