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
    using System.Collections.Generic;
    using System.Linq.Expressions;

    [Service(typeof(IArticleSearchRepository))]
    public class ArticleSearchRepository : SearchRepository, IArticleSearchRepository
    {

        public ArticleSearchRepository(ISitecoreContext sitecoreContext) : base(sitecoreContext)
        {
        }

        public SearchItems GetArticleItems(List<string> categoryNames, string storeId = "", int page = 0, int pageSize = 100, string path = "")
        {
            using (var searchContext = ContentSearchManager.GetIndex(this.IndexName).CreateSearchContext())
            {
                var query = PredicateBuilder.True<ArticleIndex>();
                var queryCategory = PredicateBuilder.False<ArticleIndex>();
                if (categoryNames.Any())
                {
                    foreach (var categoryName in categoryNames)
                    {
                        queryCategory = queryCategory.Or(x => x.ArticleCategory.Equals(categoryName, StringComparison.OrdinalIgnoreCase));
                    }
                    query = query.And(queryCategory);
                }
                if (!string.IsNullOrEmpty(storeId))
                {
                    query = query.And(x => x.Store.Equals(storeId, StringComparison.OrdinalIgnoreCase));
                }
                else // get all
                {
                    query = query.And(x => !x.ArticleCategory.Equals(FrasersContent.Constants.SpecialEventsName, StringComparison.OrdinalIgnoreCase));
                }
                if (!string.IsNullOrEmpty(path))
                {
                    query = query.And(x => x.Path.Contains(path).Boost(DefaultBoost));
                }

                return GetResults(searchContext, query, page, pageSize);
            }
        }

        public SearchItems GetArticleItems(string category, Data.ID siteId, int page = 0, int pageSize = 100)
        {
            using (var searchContext = ContentSearchManager.GetIndex(this.IndexName).CreateSearchContext())
            {
                var query = PredicateBuilder.True<ArticleIndex>();
                query = query.And(x => x.ArticleCategory.Equals(category.Trim(), StringComparison.OrdinalIgnoreCase));
                query = query.And(x => x.MallSite.Contains(IdHelper.NormalizeGuid(siteId)));

                return GetResults(searchContext, query, page, pageSize);
            }
        }

        private SearchItems GetResults<T>(IProviderSearchContext searchContext, Expression<Func<T, bool>> query, int page = 0, int pageSize = 100) where T : ArticleIndex
        {
            var today = DateUtil.ToUniversalTime(DateTime.Now);
            query = query.And(x => !x.ExpiryDateHasValue || (x.ExpiryDateHasValue && x.ExpiryDate >= today));
            query = query.And(x => !x.PostDateHasValue || (x.PostDateHasValue && x.PostDate <= today));

            var results = searchContext.GetQueryable<T>()
                   .Where(query)
                   .Page(page, pageSize)
                   .Filter(x => x.IsLatestVersion)
                   .Filter(x => x.Language == this._sitecoreContext.Language.Name)
                   .Filter(x => x.AllTemplates.Contains(IdHelper.NormalizeGuid(FrasersContent.Templates.Article.ID)))
                   .OrderByDescending(x => x.PostDate).ThenBy(x => x.Title)
                   .GetResults();

            var items = this.BuildResultModel(results);
            var hits = results.TotalSearchResults;
            return new SearchItems(items, hits);
        }
    }
}