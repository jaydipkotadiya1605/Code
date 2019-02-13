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

    [Service(typeof(IStoreOfferRepository))]
    public class StoreOfferRepository : SearchRepository, IStoreOfferRepository
    {
        public StoreOfferRepository(ISitecoreContext sitecoreContext) : base(sitecoreContext)
        {
        }

        public SearchItems GetStoreOffers()
        {
            using (var searchContext = ContentSearchManager.GetIndex(this.IndexName).CreateSearchContext())
            {
                var results = searchContext.GetQueryable<StoreOfferIndex>()
                    .Where(x => x.AllTemplates.Contains(IdHelper.NormalizeGuid(Templates.StoreOffer.ID)))
                    .Filter(x => x.IsLatestVersion && x.Language == this._sitecoreContext.Language.Name)
                    .GetResults();

                var items = this.BuildResultModel(results);
                var hits = results.TotalSearchResults;
                return new SearchItems(items, hits);
            }
        }
    }
}