namespace Sitecore.Foundation.Search.Repositories
{
    using System.Linq;
    using Sitecore.ContentSearch;
    using Sitecore.ContentSearch.Linq;
    using Sitecore.ContentSearch.Utilities;
    using Sitecore.Foundation.Abstractions.SitecoreContext;
    using Sitecore.Foundation.DependencyInjection;
    using Sitecore.Foundation.FrasersContent;
    using Sitecore.Foundation.Search.Models;
    using Sitecore.Foundation.Search.Models.Index;

    [Service(typeof(IStoreCategoryRepository))]
    public class StoreCategoryRepository : SearchRepository, IStoreCategoryRepository
    {
        public StoreCategoryRepository(ISitecoreContext sitecoreContext) : base(sitecoreContext)
        {
        }

        public SearchItems GetAllCategories()
        {
            using (var searchContext = ContentSearchManager.GetIndex(this.IndexName).CreateSearchContext())
            {
                var results = searchContext.GetQueryable<StoreCategoryIndex>()
                    .Where(x => x.AllTemplates.Contains(IdHelper.NormalizeGuid(Templates.StoreCategory.ID)))
                    .Filter(x=>x.IsLatestVersion && x.Language == this._sitecoreContext.Language.Name)
                    .GetResults();

                var items = this.BuildResultModel(results);
                var hits = results.TotalSearchResults;
                return new SearchItems(items, hits);
            }
        }
    }
}