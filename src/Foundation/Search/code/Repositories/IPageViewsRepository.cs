using Sitecore.Data;
using Sitecore.Foundation.Search.Models;

namespace Sitecore.Foundation.Search.Repositories
{
    public interface IPageViewsRepository : ISearchRepository
    {
        SearchItems GetPageViews(ID template);
    }
}