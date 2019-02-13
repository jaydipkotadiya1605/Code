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

    [Service(typeof(IBlogSearchRepository))]
    public class BlogSearchRepository : SearchRepository, IBlogSearchRepository
    {
        public BlogSearchRepository(ISitecoreContext sitecoreContext) : base(sitecoreContext)
        {
        }

        public SearchItems GetBlogItems(List<string> categoryNames, int page = 0, int pageSize = 100, string path = "")
        {
            using (var searchContext = ContentSearchManager.GetIndex(this.IndexName).CreateSearchContext())
            {
                var query = PredicateBuilder.True<BlogIndex>();
                var queryCategory = PredicateBuilder.False<BlogIndex>();

                if (categoryNames.Any())
                {
                    foreach (var categoryName in categoryNames)
                    {
                        queryCategory = queryCategory.Or(x => x.Category.Contains(categoryName));
                    }
                    query = query.And(queryCategory);
                }

                if (!string.IsNullOrEmpty(path))
                {
                    query = query.And(x => x.Path.Contains(path).Boost(DefaultBoost));
                }

                var today = DateUtil.ToUniversalTime(DateTime.Now);
                query = query.And(x => !x.ExpiryDateHasValue || (x.ExpiryDateHasValue && x.ExpiryDate >= today));
                query = query.And(x => !x.PostDateHasValue || (x.PostDateHasValue && x.PostDate <= today));

                var results = searchContext.GetQueryable<BlogIndex>()
                    .Where(query)
                    .Page(page, pageSize)
                    .Filter(x => x.IsLatestVersion)
                    .Filter(x => x.ItemId != this._sitecoreContext.Item.ID)
                    .Filter(x => x.Language == this._sitecoreContext.Language.Name)
                    .Filter(x => x.AllTemplates.Contains(IdHelper.NormalizeGuid(FrasersContent.Templates.Blog.ID)))
                    .OrderByDescending(x => x.PostDate).ThenBy(x => x.Title)
                    .GetResults();

                var items = this.BuildResultModel(results);
                var hits = results.TotalSearchResults;
                return new SearchItems(items, hits);
            }
        }

    }
}