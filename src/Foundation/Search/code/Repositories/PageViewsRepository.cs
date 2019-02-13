using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.Utilities;
using Sitecore.Data;
using Sitecore.Foundation.Abstractions.SitecoreContext;
using Sitecore.Foundation.DependencyInjection;
using Sitecore.Foundation.Search.Models;
using Sitecore.Foundation.Search.Models.Index;
using System.Linq;

namespace Sitecore.Foundation.Search.Repositories
{
    [Service(typeof(IPageViewsRepository))]
    public class PageViewsRepository : SearchRepository, IPageViewsRepository
    {
        public PageViewsRepository(ISitecoreContext sitecoreContext) : base(sitecoreContext)
        {
        }

        public SearchItems GetPageViews(ID template)
        {
            using (var searchContext = ContentSearchManager.GetIndex(this.IndexName).CreateSearchContext())
            {
                var results = searchContext.GetQueryable<PageViewsIndex>()
                    .Where(x => x.AllTemplates.Contains(IdHelper.NormalizeGuid(template)))
                    .Filter(x => x.IsLatestVersion && x.Language == this._sitecoreContext.Language.Name)
                    .GetResults();

                var items = this.BuildResultModel(results);
                var hits = results.TotalSearchResults;
                return new SearchItems(items, hits);
            }
        }
    }
}