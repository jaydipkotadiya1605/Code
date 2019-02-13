namespace Sitecore.Foundation.Search.Repositories
{
    using System;
    using System.Linq;
    using Sitecore.ContentSearch;
    using Sitecore.ContentSearch.Linq;
    using Sitecore.ContentSearch.Utilities;
    using Sitecore.Foundation.Abstractions.SitecoreContext;
    using Sitecore.Foundation.DependencyInjection;
    using Sitecore.Foundation.Search.Models;
    using Sitecore.Foundation.Search.Models.Index;
    using Sitecore.ContentSearch.Linq.Utilities;

    [Service(typeof(ISiteSearchRepository))]
    public class SiteSearchRepository : SearchRepository, ISiteSearchRepository
    {
        public SiteSearchRepository(ISitecoreContext sitecoreContext) : base(sitecoreContext)
        {
        }

        public SearchItems GetSitebySiteCode(string siteCode)
        {
            using (var searchContext = ContentSearchManager.GetIndex(this.IndexName).CreateSearchContext())
            {
                var querySite = PredicateBuilder.True<SiteIndex>();
                if (!string.IsNullOrEmpty(siteCode))
                {
                    querySite = querySite.And(x => x.SiteCode.Equals(siteCode, StringComparison.OrdinalIgnoreCase));
                }
                var results = searchContext.GetQueryable<SiteIndex>().Where(querySite).Filter(x => x.IsLatestVersion)
                    .Filter(x => x.Language == this._sitecoreContext.Language.Name)
                    .GetResults();
                var items = this.BuildResultModel(results);
                var hits = results.TotalSearchResults;
                return new SearchItems(items, hits);
            }
        }
    }
}