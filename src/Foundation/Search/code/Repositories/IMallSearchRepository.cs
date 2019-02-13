namespace Sitecore.Foundation.Search.Repositories
{
    using Sitecore.Foundation.Search.Models;

    public interface IMallSearchRepository : ISearchRepository
    {
        SearchItems GetAllMalls();
    }
}