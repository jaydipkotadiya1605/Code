using Sitecore.Foundation.Search.Models;

namespace Sitecore.Foundation.Search.Repositories
{
    public interface ISiteSearchRepository : ISearchRepository
    {
        SearchItems GetSitebySiteCode(string siteCode);
    }
}