namespace Sitecore.Foundation.Search.Repositories
{
    using Sitecore.Foundation.Search.Models;

    public interface IWingRepository : ISearchRepository
    {
        SearchItems GetAllWings();
    }
}