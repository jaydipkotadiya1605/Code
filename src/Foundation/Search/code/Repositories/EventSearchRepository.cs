namespace Sitecore.Foundation.Search.Repositories
{
    using Sitecore.ContentSearch;
    using Sitecore.ContentSearch.Linq;
    using Sitecore.Foundation.Abstractions.SitecoreContext;
    using Sitecore.Foundation.DependencyInjection;
    using Sitecore.Foundation.Search.Models;
    using Sitecore.Foundation.Search.Models.Index;
    using System.Linq;
    using Sitecore.ContentSearch.Linq.Utilities;
    using Sitecore.ContentSearch.Utilities;
    using System;

    [Service(typeof(IEventSearchRepository))]
    public class EventSearchRepository : SearchRepository, IEventSearchRepository
    {
        public EventSearchRepository(ISitecoreContext sitecoreContext) : base(sitecoreContext)
        {
        }

        public SearchItems GetEventItems(string categoryName, string mall, int page = 0, int pageSize = 100, string path = "")
        {
            using (var searchContext = ContentSearchManager.GetIndex(this.IndexName).CreateSearchContext())
            {
                var query = PredicateBuilder.True<EventIndex>();
                var queryCategory = PredicateBuilder.False<EventIndex>();

                if (!string.IsNullOrEmpty(categoryName))
                {
                    queryCategory = queryCategory.Or(x => x.Category.Equals(categoryName, StringComparison.OrdinalIgnoreCase) && !x.IsSpecialEvent);
                    queryCategory = queryCategory.Or(x => x.IsSpecialEvent && categoryName.Equals(FrasersContent.Constants.SpecialEventsName, StringComparison.OrdinalIgnoreCase));
                    query = query.And(queryCategory);
                }

                if (!string.IsNullOrEmpty(mall))
                {
                    query = query.And(x => x.Malls.Contains(mall).Boost(DefaultBoost));
                }

                if (!string.IsNullOrEmpty(path))
                {
                    query = query.And(x => x.Path.Contains(path).Boost(DefaultBoost));
                }

                var today = DateUtil.ToUniversalTime(DateTime.Now);
                query = query.And(x => !x.ExpiryDateHasValue || (x.ExpiryDateHasValue && x.ExpiryDate >= today));
                query = query.And(x => !x.PostDateHasValue || (x.PostDateHasValue && x.PostDate <= today));

                var results = searchContext.GetQueryable<EventIndex>()
                    .Where(query)
                    .Page(page, pageSize)
                    .Filter(x => x.IsLatestVersion)
                    .Filter(x => x.Language == this._sitecoreContext.Language.Name)
                    .Filter(x => x.AllTemplates.Contains(IdHelper.NormalizeGuid(FrasersContent.Templates.Event.ID)))
                    .OrderBy(x => x.StartDate).ThenBy(x => x.EndDate).ThenBy(x => x.MallName).ThenBy(x => x.Title)
                    .GetResults();

                var items = this.BuildResultModel(results);
                var hits = results.TotalSearchResults;
                return new SearchItems(items, hits);
            }
        }

        public SearchItems GetEventAndArticleItems(string categoryName, string mall, int page = 0, int pageSize = 100, string path = "")
        {
            var aaa = GetEventItems(categoryName, mall, page, pageSize, path);
            using (var searchContext = ContentSearchManager.GetIndex(this.IndexName).CreateSearchContext())
            {

            }


                return aaa;
        }

    }
}