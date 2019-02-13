using Sitecore.Foundation.Search.Models;
using System.Collections.Generic;

namespace Sitecore.Foundation.Search.Repositories
{
    public interface IArticleSearchRepository : ISearchRepository
    {
        SearchItems GetArticleItems(List<string> categoryNames, string storeId = "", int page = 0, int pageSize = 100, string path = "");
        SearchItems GetArticleItems(string category, Data.ID siteId, int page = 0, int pageSize = 100);
    }
}