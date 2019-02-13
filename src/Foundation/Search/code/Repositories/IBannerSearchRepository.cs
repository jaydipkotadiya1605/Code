using Sitecore.Data;
using Sitecore.Foundation.Search.Models;

namespace Sitecore.Foundation.Search.Repositories
{
    public interface IBannerSearchRepository : ISearchRepository
    {
        SearchItems GetBannerItems(ID mallID, int page = 0, int pageSize = 100);
        SearchItems GetBannerBySiteCode(string siteCode, int page = 0, int pageSize = 100);
    }
}