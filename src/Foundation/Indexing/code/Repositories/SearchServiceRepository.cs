namespace Sitecore.Foundation.Indexing.Repositories
{
    using Sitecore.Foundation.DependencyInjection;
    using Sitecore.Foundation.Indexing.Models;
    using Sitecore.Foundation.Indexing.Services;

    [Service(typeof(ISearchServiceRepository))]
    public class SearchServiceRepository : ISearchServiceRepository
    {
        public virtual SearchService Get(ISearchSettings searchSettings)
        {
            return new SearchService(searchSettings);
        }
    }
}