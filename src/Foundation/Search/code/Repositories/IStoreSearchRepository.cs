namespace Sitecore.Foundation.Search.Repositories
{
    using Sitecore.Foundation.Search.Models;
    using Sitecore.Foundation.Search.Models.Index;
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public interface IStoreSearchRepository : ISearchRepository
    {
        SearchItems Search(
            Expression<Func<StoreIndex, bool>> query,
            int pageIndex,
            int pageSize,
            Func<IQueryable<StoreIndex>, IQueryable<StoreIndex>> orderFunc = null);
    }
}