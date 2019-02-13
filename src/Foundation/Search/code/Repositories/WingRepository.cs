namespace Sitecore.Foundation.Search.Repositories
{
    using Sitecore.ContentSearch;
    using Sitecore.ContentSearch.Linq;
    using Sitecore.ContentSearch.Utilities;
    using Sitecore.Foundation.Abstractions.SitecoreContext;
    using Sitecore.Foundation.DependencyInjection;
    using Sitecore.Foundation.FrasersContent;
    using Sitecore.Foundation.Search.Models;
    using Sitecore.Foundation.Search.Models.Index;
    using System.Linq;

    [Service(typeof(IWingRepository))]
    public class WingRepository : SearchRepository, IWingRepository
    {
        public WingRepository(ISitecoreContext sitecoreContext) : base(sitecoreContext)
        {
        }

        public SearchItems GetAllWings()
        {
            using (var searchContext = ContentSearchManager.GetIndex(this.IndexName).CreateSearchContext())
            {
                var results = searchContext.GetQueryable<StoreWingIndex>()
                    .Where(x => x.AllTemplates.Contains(IdHelper.NormalizeGuid(Templates.Wing.ID)))
                    .Filter(x => x.IsLatestVersion && x.Language == this._sitecoreContext.Language.Name)
                    .GetResults();

                var items = this.BuildResultModel(results);
                var hits = results.TotalSearchResults;
                return new SearchItems(items, hits);
            }
        }
    }
}