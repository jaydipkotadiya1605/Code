namespace Sitecore.Foundation.Search.Repositories
{
    using Sitecore.Foundation.Search.Models;

    public interface IStoreCategoryRepository : ISearchRepository
    {
        SearchItems GetAllCategories();
    }
}