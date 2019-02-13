namespace Sitecore.Foundation.Search.Repositories
{
    using Sitecore.Foundation.Search.Models;
    using System.Collections.Generic;

    public interface IEventAndArticleRepository : ISearchRepository
    {
        SearchItems GetEventItems(string categoryName, string mall, int page = 0, int pageSize = 100, string path = "");

        SearchItems GetArticleItems(List<string> categoryNames, string storeId = "", int page = 0, int pageSize = 100, string path = "");

        SearchItems GetEventAndArticleItems(string categoryName, string mall, int page = 0, int pageSize = 100, string path = "");
    }
}