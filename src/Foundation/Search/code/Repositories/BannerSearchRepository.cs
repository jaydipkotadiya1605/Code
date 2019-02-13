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
    using Sitecore.Data;
    using System.Collections.Generic;

    [Service(typeof(IBannerSearchRepository))]
    public class BannerSearchRepository : SearchRepository, IBannerSearchRepository
    {
        private ISiteSearchRepository _siteSearchRepository { get; }
        public BannerSearchRepository(ISitecoreContext sitecoreContext, ISiteSearchRepository siteSearchRepository) : base(sitecoreContext)
        {
            _siteSearchRepository = siteSearchRepository;
        }

        private SearchItems QueryBanner(IProviderSearchContext searchContext, string path, int page = 0, int pageSize = 100)
        {
            var query = PredicateBuilder.True<BannerIndex>();

            if (!string.IsNullOrEmpty(path))
            {
                query = query.And(x => x.Path.Contains(path).Boost(DefaultBoost));
            }

            var today = DateUtil.ToUniversalTime(DateTime.Now);
            query = query.And(x => !x.ExpiryDateHasValue || (x.ExpiryDateHasValue && x.ExpiryDate >= today));
            query = query.And(x => !x.PostDateHasValue || (x.PostDateHasValue && x.PostDate <= today));

            var results = searchContext.GetQueryable<BannerIndex>()
                .Where(query)
                .Page(page, pageSize)
                .Filter(x => x.IsLatestVersion)
                .Filter(x => x.Language == this._sitecoreContext.Language.Name)
                .Filter(x => x.AllTemplates.Contains(IdHelper.NormalizeGuid(FrasersContent.Templates.Banner.ID)))
                .GetResults();

            var items = this.BuildResultModel(results);
            var hits = results.TotalSearchResults;
            return new SearchItems(items, hits);
        }

        public SearchItems GetBannerItems(ID mallID, int page = 0, int pageSize = 100)
        {
            var path = _sitecoreContext.Database.GetItem(mallID).Paths.FullPath;
            using (var searchContext = ContentSearchManager.GetIndex(this.IndexName).CreateSearchContext())
            {
                return QueryBanner(searchContext, path, page, pageSize);
            }
        }

        public SearchItems GetBannerBySiteCode(string siteCode, int page = 0, int pageSize = 100)
        {
            var siteItems = _siteSearchRepository.GetSitebySiteCode(siteCode);
            if (siteItems.TotalNumberOfResults == 1)
            {
                using (var searchContext = ContentSearchManager.GetIndex(this.IndexName).CreateSearchContext())
                {
                    var path = siteItems.ScoredItems.First().Item.Paths.FullPath;
                    return QueryBanner(searchContext, path, page, pageSize);
                }
            }
            return new SearchItems(new List<ScoredItem>(), 0);
        }
    }
}