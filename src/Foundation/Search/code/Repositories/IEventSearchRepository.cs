using Sitecore.Foundation.Search.Models;

namespace Sitecore.Foundation.Search.Repositories
{
    public interface IEventSearchRepository : ISearchRepository
    {
        SearchItems GetEventItems(string categoryName, string mall, int page = 0, int pageSize = 100, string path = "");

    }
}