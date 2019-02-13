using Sitecore.Foundation.Search.Models;
using System.Collections.Generic;

namespace Sitecore.Foundation.Search.Repositories
{
    public interface IBlogSearchRepository : ISearchRepository
    {
        SearchItems GetBlogItems(List<string> categoryNames, int page = 0, int pageSize = 100, string path = "");
    }
}