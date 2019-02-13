namespace Sitecore.Feature.Search.Services
{
    using System;
    using Sitecore.ContentSearch.Linq.Utilities;
    using Sitecore.ContentSearch.Utilities;
    using Sitecore.Feature.Search.ViewModels;
    using Sitecore.Foundation.Abstractions.SitecoreContext;
    using Sitecore.Foundation.DependencyInjection;
    using Sitecore.Foundation.Search.Models.Index;
    using Sitecore.Foundation.Search.Repositories;
    using Sitecore.Foundation.SitecoreExtensions.Extensions;
    using Sitecore.Foundation.SitecoreExtensions.Repositories;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web;
    using Sitecore.ContentSearch.SearchTypes;
    using Sitecore.ExperienceExplorer.Business.Utilities.Extensions;
    using Sitecore.Feature.Search.Models;
    using Sitecore.Foundation.ItemResolver.Extensions;
    using Sitecore.Foundation.Multisite.Providers;

    [Service(typeof(ISearchService))]
    public class SearchService : ISearchService
    {
        private readonly IRenderingPropertiesRepository renderingPropertiesRepository;
        private readonly IGlobalSearchRepository globalSearchRepository;
        private readonly ISitecoreContext sitecoreContext;
        private readonly ISiteSettingsProvider siteSettingsProvider;

        public SearchService(
            IGlobalSearchRepository globalSearchRepository,
            IRenderingPropertiesRepository renderingPropertiesRepository,
            ISitecoreContext sitecoreContext,
            ISiteSettingsProvider siteSettingsProvider)
        {
            this.globalSearchRepository = globalSearchRepository;
            this.renderingPropertiesRepository = renderingPropertiesRepository;
            this.sitecoreContext = sitecoreContext;
            this.siteSettingsProvider = siteSettingsProvider;
        }

        public FraserRewardsViewModel GetLatestFraserRewards(FraserRewardsFilterModel criteria)
        {
            return this.ExecuteSearch(criteria, queryable => queryable.OrderByDescending(x => x.PostDate).ThenBy(x => x.Title));
        }

        public FraserRewardsViewModel Search(FraserRewardsFilterModel criteria)
        {
            return this.ExecuteSearch(criteria, queryable => queryable
                .OrderByDescending(x => x[Foundation.FrasersContent.Constants.Score_FieldName])
                .ThenBy(x => x.Title));
        }

        public SearchHeaderViewModel GetSearchHeader(Mvc.Presentation.Rendering rendering)
        {
            var model = this.renderingPropertiesRepository.Get<SearchHeaderViewModel>(rendering);
            var datasource = this.sitecoreContext.DataSourceItem;
            if (datasource.IsDerived(Templates.SearchHeader.ID))
            {
                model.Title = datasource.Field(Templates.SearchHeader.Fields.Title);
                model.Subtitle = datasource.Field(Templates.SearchHeader.Fields.Subtitle);
                model.PlaceholderText = datasource.GetString(Templates.SearchHeader.Fields.PlaceholderText);
                model.ButtonSearchText = datasource.Field(Templates.SearchHeader.Fields.ButtonSearchText);
                if (!string.IsNullOrEmpty(model.SearchResultPage))
                {
                    var id = HttpUtility.UrlDecode(model.SearchResultPage);
                    model.SearchResultPageUrl = this.sitecoreContext.Database.GetItem(id)?.Url();
                }
            }
            return model;
        }

        public SearchHeaderViewModel GetSearchOverlay(Mvc.Presentation.Rendering rendering)
        {
            var model = this.renderingPropertiesRepository.Get<SearchHeaderViewModel>(rendering);
            var url = this.GetSearchOverlayResultPage();
            model.SearchResultPageUrl = url;
            return model;
        }

        private Expression<Func<T, bool>> FilterExpiryDate<T>() where T: SchedulableItemIndex
        {
            var filterExpiryDate = PredicateBuilder.False<T>();
            filterExpiryDate = filterExpiryDate.Or(x => x.ExpiryDateHasValue && x.ExpiryDate >= DateTime.UtcNow);
            filterExpiryDate = filterExpiryDate.Or(x => !x.ExpiryDateHasValue);
            return filterExpiryDate;
        }

        private Expression<Func<T, bool>> FilterPostDate<T>() where T : SchedulableItemIndex
        {
            var filterPostDate = PredicateBuilder.False<T>();
            filterPostDate = filterPostDate.Or(x => x.PostDateHasValue && x.PostDate <= DateTime.UtcNow);
            filterPostDate = filterPostDate.Or(x => !x.PostDateHasValue);
            return filterPostDate;
        }

        private Expression<Func<FraserRewardsIndex, bool>> BuildQuery(FraserRewardsFilterModel filter)
        {
            var query = PredicateBuilder.True<FraserRewardsIndex>();

            if (filter.SelectedTypes != null)
            {
                var queryTemplates = PredicateBuilder.False<FraserRewardsIndex>();
                filter.SelectedTypes.ForEach(template =>
                {
                    queryTemplates = queryTemplates.Or(x => x.AllTemplates.Contains(template));
                });
                query = query.And(queryTemplates);
            }

            if (filter.IsFilterOnCurrentSite)
            {
                query = query.And(x => x.Path.Contains(this.sitecoreContext.Site.RootPath));
            }

            query = AddContentPredicates(query, new SearchQuery {QueryText = filter.Keyword});

            return query.And(x => x.HasSearchResultFormatter);
        }

        private Expression<Func<FraserRewardsIndex, bool>> BuildFilter()
        {
            return PredicateBuilder.True<FraserRewardsIndex>()
                .And(this.FilterExpiryDate<FraserRewardsIndex>())
                .And(this.FilterPostDate<FraserRewardsIndex>());
        }

        private static Expression<Func<T, bool>> AddContentPredicates<T>(Expression<Func<T, bool>> query, SearchQuery searchQuery) where T: SearchResultItem
        {
            if (string.IsNullOrEmpty(searchQuery.QueryText))
            {
                return query;
            }

            var contentPredicates = PredicateBuilder.False<T>();
            foreach (var provider in Foundation.Indexing.Repositories.IndexingProviderRepository.QueryPredicateProviders)
            {
                contentPredicates = contentPredicates.Or(provider.GetQueryPredicate<T>(searchQuery));
            }
            return query.And(contentPredicates);
        }

        private FraserRewardsViewModel ExecuteSearch(FraserRewardsFilterModel criteria, Func<IQueryable<FraserRewardsIndex>, IQueryable<FraserRewardsIndex>> orderFunc)
        {
            var query = this.BuildQuery(criteria);
            var pages = (criteria.PageIndex * criteria.PageSize);

            var filter = this.BuildFilter();

            var results = this.globalSearchRepository.Search(query, filter, 0, pages, orderFunc);

            return new FraserRewardsViewModel
            {
                TotalNumberOfResults = results.TotalNumberOfResults,
                PageIndex = criteria.PageIndex,
                PageSize = criteria.PageSize,
                Results = results.Results,
                Title = criteria.Title,
                NotFoundMessage = criteria.NotFoundMessage,
                IsHiddenIfNotFound = (results.TotalNumberOfResults == 0 && criteria.IsHiddenResultIfNotFound)
            };
        }

        private string GetSearchOverlayResultPage()
        {
            var settings = this.siteSettingsProvider.GetSetting(this.sitecoreContext.SiteRoot, Templates.SearchSettings.ID);
            if (settings == null) return this.sitecoreContext.SiteRoot.Url();

            var resultResultPage = settings.GetFieldAsLinkedItem(Templates.SearchSettings.Fields.OverlaySearchResultPage);
            return resultResultPage?.Url();

        }
    }
}