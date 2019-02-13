namespace Sitecore.Feature.Store.Services
{
    using System;
    using Sitecore.ContentSearch.Utilities;
    using Sitecore.Feature.Store.Models;
    using Sitecore.Foundation.Abstractions.SitecoreContext;
    using Sitecore.Foundation.DependencyInjection;
    using Sitecore.Foundation.Dictionary.Repositories;
    using Sitecore.Foundation.Search.Repositories;
    using Sitecore.Foundation.SitecoreExtensions.Extensions;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web;
    using Sitecore.ContentSearch.Linq;
    using Sitecore.ContentSearch.Linq.Utilities;
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Sitecore.Foundation.FrasersContent;
    using Sitecore.Foundation.Search.Models;
    using Sitecore.Foundation.Search.Models.Index;
    using Sitecore.Foundation.SitecoreExtensions.Repositories;
    using Sitecore.Foundation.Multisite.Providers;

    [Service(typeof(IStoreService))]
    public class StoreService : IStoreService
    {
        private readonly ISitecoreContext sitecoreContext;
        private readonly IStoreSearchRepository storeSearchRepository;
        private readonly IRenderingPropertiesRepository renderingPropertiesRepository;
        private readonly ISiteSettingsProvider siteSettingsProvider;

        public StoreService(
            ISitecoreContext sitecoreContext,
            IStoreSearchRepository storeSearchRepository,
            IRenderingPropertiesRepository renderingPropertiesRepository,
            ISiteSettingsProvider siteSettingsProvider)
        {
            this.sitecoreContext = sitecoreContext;
            this.storeSearchRepository = storeSearchRepository;
            this.renderingPropertiesRepository = renderingPropertiesRepository;
            this.siteSettingsProvider = siteSettingsProvider;
        }

        public IEnumerable<AlphabetFilterViewModel> GetAlphabetFilters()
        {
            var results = this.sitecoreContext.Database
                .GetItem(Store.Constants.AlphabetDatasources)
                .Children
                .Where(x => x.IsDerived(Store.Templates.Alphabet.ID) && x.HasContextLanguage())
                .Select(x => new AlphabetFilterViewModel
                {
                    Id = IdHelper.NormalizeGuid(x.ID),
                    Name = x.GetString(Store.Templates.Alphabet.Fields.Text),
                    Keyword = x.GetString(Store.Templates.Alphabet.Fields.Keyword)
                })
                .ToList();

            return results;
        }

        public IEnumerable<StoreCategoryViewModel> GetAllCategories()
        {
            var model = this.sitecoreContext.Database.GetItem(new ID(Store.Constants.StoreCategoryDatasource))
                .Children
                .Where(x => x.IsDerived(Templates.StoreCategory.ID))
                .Select(x => new StoreCategoryViewModel
                {
                    Id = IdHelper.NormalizeGuid(x.ID),
                    Name = x.GetString(Templates.StoreCategory.Fields.Value)
                })
                .OrderBy(x => x.Name)
                .ToList();

            model.Insert(0, new StoreCategoryViewModel
            {
                Id = string.Empty,
                Name = DictionaryPhraseRepository.Current.Get(Store.Constants.All, Store.Constants.AllText)
            });
            return model;
        }

        public IEnumerable<WingViewModel> GetAllWings(string mallId)
        {
            mallId = !string.IsNullOrWhiteSpace(mallId) ? mallId : GetSiteId();
            var results = this.sitecoreContext.Database.GetItem(new ID(Store.Constants.StoreWingDatasource))
                .Children
                .Where(x => x.IsDerived(Templates.Wing.ID));
            List<Item> wingItemList = new List<Item>();
            foreach (var eachWingItem in results)
            {
                List<string> hideInSites = ((Sitecore.Data.Fields.MultilistField)eachWingItem.Fields[Sitecore.Feature.Store.Templates.DisplayOption.Fields.HideInSites]).GetItems()
                                            .Select(x => IdHelper.NormalizeGuid(x.ID)).ToList();
                if (!string.IsNullOrWhiteSpace(mallId))
                {
                    if (!hideInSites.Contains(mallId))
                    {
                        wingItemList.Add(eachWingItem);
                    }
                }
                else
                {
                    wingItemList.Add(eachWingItem);
                }
            }

            var wingList = wingItemList.Select(x => new WingViewModel
            {
                Id = IdHelper.NormalizeGuid(x.ID),
                Name = x.GetString(Templates.Wing.Fields.Value)
            })
              .OrderBy(x => x.Name)
              .ToList();

            if (wingList.Count > 0)
            {
                wingList.Insert(0, new WingViewModel
                {
                    Id = string.Empty,
                    Name = DictionaryPhraseRepository.Current.Get(Store.Constants.All, Store.Constants.AllText)
                });
            }

            return wingList;
        }

        public IEnumerable<StoreOfferViewModel> GetAllStoreOffers(string mallId)
        {
            mallId = !string.IsNullOrWhiteSpace(mallId) ? mallId : GetSiteId();
            List<Item> storeOffers = this.sitecoreContext.Database.GetItem(new ID(Store.Constants.StoreOfferDatasource))
                .Children
                .Where(x => x.IsDerived(Templates.StoreOffer.ID)).ToList();

            List<Item> storeOfferList = new List<Item>();
            foreach (var eachOffer in storeOffers)
            {
                List<string> hideInSites = ((Sitecore.Data.Fields.MultilistField)eachOffer.Fields[Store.Templates.DisplayOption.Fields.HideInSites]).GetItems()
                                            .Select(x => IdHelper.NormalizeGuid(x.ID)).ToList();
                if (!string.IsNullOrWhiteSpace(mallId))
                {
                    if (!hideInSites.Contains(mallId))
                    {
                        storeOfferList.Add(eachOffer);
                    }
                }
                else
                {
                    storeOfferList.Add(eachOffer);
                }
            }

            var model = storeOfferList.OrderBy(x => x.Fields[Templates.StoreOffer.Fields.OrderNumber].Value)
                .Select(x => new StoreOfferViewModel
                {
                    Id = IdHelper.NormalizeGuid(x.ID),
                    Name = x.GetString(Templates.StoreOffer.Fields.Value)
                })
                .ToList();

            model.Insert(model.Count, new StoreOfferViewModel
            {
                Id = Store.Constants.StoreOfferNewStatus,
                Name = DictionaryPhraseRepository.Current.Get(Store.Constants.NewStore, Store.Constants.NewStoreText)
            });
            return model;
        }

        public StoreQuicFinderViewModel GetSearchResultPage(Mvc.Presentation.Rendering rendering)
        {
            var option = this.siteSettingsProvider.GetUrlOptions();
            var homeUrl = this.sitecoreContext.SiteRoot.Url(option);
            var url = string.Empty;

            var storeQuicFinderViewModel = this.renderingPropertiesRepository.Get<StoreQuicFinderViewModel>(rendering) ?? new StoreQuicFinderViewModel();

            if (!string.IsNullOrEmpty(storeQuicFinderViewModel.SearchResultPage))
            {
                var id = storeQuicFinderViewModel.SearchResultPage;
                url = this.sitecoreContext.Database.GetItem(id)?.Url(option);
            }
            storeQuicFinderViewModel.SearchResultPageUrl = url ?? homeUrl;

            return storeQuicFinderViewModel;
        }

        public StoreSearchResultViewModel Search(SearchCriteria criteria)
        {
            var query = PredicateBuilder.True<StoreIndex>();

            if (!string.IsNullOrEmpty(criteria.CategoryId))
            {
                query = query.And(x => x.Categories.Contains(criteria.CategoryId));
            }

            if (!string.IsNullOrEmpty(criteria.WingId))
            {
                query = query.And(x => x.Wing.Equals(criteria.WingId));
            }

            if (!string.IsNullOrEmpty(criteria.MallId))
            {
                query = query.And(x => x.MallSite.Equals(criteria.MallId));
            }

            query = this.FilterByAlphabet(criteria, query);
            query = this.FilterByOffers(criteria, query);
            query = this.FilterByExpiryDate(query);
            query = this.FilterByKeyword(criteria, query);

            var pageSize = criteria.PageIndex * criteria.PageSize;
            var results = this.storeSearchRepository.Search(
                query,
                0,
                pageSize,
                queryable => queryable.OrderByDescending(x => x[Templates.Store.Fields.Score_FieldName]).ThenBy(x => x.StoreName));

            var stores = results.ScoredItems.Select(this.ConvertToView);
            var hasMoreResult = pageSize < results.TotalNumberOfResults;

            return new StoreSearchResultViewModel
            {
                PageIndex = criteria.PageIndex,
                PageSize = criteria.PageSize,
                TotalNumberOfResults = results.TotalNumberOfResults,
                Stores = stores,
                HasMoreResult = hasMoreResult,
                Keyword = criteria.Keyword,
                IsHiddenIfNotFound = (criteria.IsHiddenIfNotFound && results.TotalNumberOfResults == 0)
            };
        }

        public RelatedStoresViewModel GetLatestStores(RelatedStoresSearchCriteria criteria)
        {
            var query = PredicateBuilder.True<StoreIndex>();
            var pageIndex = criteria.PageIndex > 0 ? criteria.PageIndex : 1;
            var pageSize = pageIndex * criteria.PageSize;

            query = this.FilterByExpiryDate(query);

            query = query.And(x => x.ItemId != criteria.CurrentStoreId);

            if (!ID.IsNullOrEmpty(criteria.CurrentMallId))
            {
                query = query.And(x => x.MallSite.Contains(IdHelper.NormalizeGuid(criteria.CurrentMallId)));
            }

            var results = this.storeSearchRepository.Search(query, 0, pageSize,
                queryable => queryable.OrderByDescending(x => x.PageViews).ThenByDescending(x => x.PostDate));

            return new RelatedStoresViewModel
            {
                Stores = results.ScoredItems.Select(this.ConvertToView)
            };
        }

        private Expression<Func<StoreIndex, bool>> GetFreeTextPredicate(Expression<Func<StoreIndex, bool>> queryable, string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return queryable;
            }

            var predicate = PredicateBuilder.False<StoreIndex>();
            var keywords = query.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            if (!keywords.Contains(query))
            {
                keywords.Add(query);
            }

            keywords.ForEach(keyword => predicate = predicate
                    .Or(x => x.StoreName.Contains(keyword)).Boost(400)
                    .Or(x => x.Keywords.Contains(keyword)).Boost(300)
                    .Or(x => x.Description.Contains(keyword).Boost(200))
                    .Or(x => x.Brands.Contains(keyword))
                    .Or(x => x.Categories.Contains(keyword))
                    .Or(x => x.PhoneNumber.Contains(keyword))
                    .Or(x => x.Wing.Contains(keyword))
                    .Or(x => x.UnitNo.Contains(keyword))
                    .Or(x => x.Contact.Contains(keyword))
                    .Or(x => x.OpeningHours.Contains(keyword))
            );

            return queryable.And(predicate);
        }

        private Expression<Func<StoreIndex, bool>> FilterByExpiryDate(Expression<Func<StoreIndex, bool>> queryable)
        {
            var expiryDateQuery = PredicateBuilder.False<StoreIndex>();
            expiryDateQuery = expiryDateQuery.Or(x =>
                x.StoreHasUpcomingOrNewDate &&
                x.ExpiryDate >= DateTime.UtcNow &&
                x.PostDate <= DateTime.UtcNow);

            expiryDateQuery = expiryDateQuery.Or(x => !x.StoreHasUpcomingOrNewDate && x.ExpiryDate >= DateTime.UtcNow);

            queryable = queryable.And(expiryDateQuery);
            return queryable;
        }

        private Expression<Func<StoreIndex, bool>> FilterByAlphabet(SearchCriteria criteria, Expression<Func<StoreIndex, bool>> queryable)
        {
            var keywordQuery = PredicateBuilder.False<StoreIndex>();
            if (!string.IsNullOrEmpty(criteria.AlphabetId))
            {
                var alphabetSelected = this.GetAlphabetFilters()
                    .FirstOrDefault(x => x.Name.Equals(HttpUtility.UrlDecode(criteria.AlphabetId), StringComparison.InvariantCultureIgnoreCase))?
                    .Keyword;
                if (!string.IsNullOrEmpty(alphabetSelected))
                {
                    var keywords = alphabetSelected.ToLowerInvariant().Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

                    keywords.ForEach(keyword =>
                    {
                        keywordQuery = keywordQuery.Or(x => x.Keywords.StartsWith(keyword, StringComparison.InvariantCultureIgnoreCase));
                        keywordQuery = keywordQuery.Or(x => x.StoreName.StartsWith(keyword, StringComparison.InvariantCultureIgnoreCase));
                    });
                    queryable = queryable.And(keywordQuery);
                }
            }

            return queryable;
        }

        private Expression<Func<StoreIndex, bool>> FilterByOffers(SearchCriteria criteria, Expression<Func<StoreIndex, bool>> queryable)
        {

            if (criteria.Offers == null || !criteria.Offers.Any()) return queryable;

            var offers = criteria.Offers
                .Where(x => !x.Equals(Store.Constants.StoreOfferNewStatus, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
            if (offers.Any())
            {
                foreach (var offer in offers)
                {
                    queryable = queryable.And(x => x.StoreOffers.Contains(offer));
                }

            }
            var newStoreOffer = criteria.Offers
                .FirstOrDefault(x => x.Equals(Store.Constants.StoreOfferNewStatus, StringComparison.InvariantCultureIgnoreCase));
            if (newStoreOffer != null)
            {
                queryable = queryable.And(x =>
                    x.StoreHasUpcomingOrNewDate &&
                    x.NewDate <= DateTime.UtcNow &&
                    x.NextThreeMonthsNewDate > DateTime.UtcNow);
            }

            return queryable;
        }

        private Expression<Func<StoreIndex, bool>> FilterByKeyword(SearchCriteria criteria, Expression<Func<StoreIndex, bool>> queryable)
        {
            return string.IsNullOrEmpty(criteria.Keyword) ?
                queryable :
                this.GetFreeTextPredicate(queryable, criteria.Keyword);
        }

        private StoreViewModel ConvertToView(ScoredItem scoredItem)
        {
            var item = scoredItem.Item;
            var viewModel = new StoreViewModel
            {
                Url = item.Url(),
                StoreName = item.GetString(Templates.Store.Fields.StoreName),
                Description = item.GetString(Templates.Store.Fields.Description),
                Logo = item.BuildImageItem(Templates.Store.Fields.Logo),
                PhoneNumber = item.GetString(Templates.Store.Fields.PhoneNumber),
                Contact = item.GetString(Templates.Store.Fields.Contact),
                NewDate = item.GetDateTime(Templates.Store.Fields.NewDate),
                PostDate = item.GetDateTime(Templates.SchedulableContent.Fields.PostDate),
                ExpiryDate = item.GetDateTime(Templates.SchedulableContent.Fields.ExpiryDate),
                UnitNo = item.GetString(Templates.Store.Fields.UnitNo),
                Brands = item.GetString(Templates.Store.Fields.Brands),
                Keywords = item.GetString(Templates.Store.Fields.Keywords),
                OpeningHours = item.GetString(Templates.Store.Fields.OpeningHours),
                Categories = this.ConvertToStoreCategoriesView(item),
                Offers = this.ConvertToStoreOffersView(item),
                Id = IdHelper.NormalizeGuid(item.ID)
            };
            var mallSite = item.GetAncestorOrSelfOfTemplate(Foundation.Multisite.Templates.Site.ID);
            if (mallSite != null)
            {
                viewModel.Mall = mallSite.DisplayName;
            }

            var currentdate = DateTime.UtcNow;
            if (currentdate >= viewModel.PostDate && currentdate < viewModel.NewDate)
            {
                viewModel.Status = StoreStatus.Upcoming;
            }
            else if (currentdate >= viewModel.NewDate && currentdate < (viewModel.NewDate.AddMonths(3)))
            {
                viewModel.Status = StoreStatus.New;
            }
            else
            {
                viewModel.Status = StoreStatus.Normal;
            }

            return viewModel;
        }

        private IEnumerable<StoreCategoryViewModel> ConvertToStoreCategoriesView(Item item)
        {
            var categories = item.GetMultiListValueItems(Templates.Store.Fields.StoreCategories);
            return categories.Select(x => new StoreCategoryViewModel
            {
                Id = IdHelper.NormalizeGuid(x.ID),
                Name = x.GetString(Templates.StoreCategory.Fields.Value)
            });
        }

        private IEnumerable<StoreOfferViewModel> ConvertToStoreOffersView(Item item)
        {
            var offers = item.GetMultiListValueItems(Templates.Store.Fields.StoreOffers);
            return offers.Select(x => new StoreOfferViewModel
            {
                Id = IdHelper.NormalizeGuid(x.ID),
                Name = x.GetString(Templates.StoreOffer.Fields.Value),
                IconCssClass = x.GetString(Templates.StoreOffer.Fields.IconCssClass),
            });
        }

        private string GetSiteId()
        {
            var rootItem = Sitecore.Context.Item.Axes.GetAncestors().LastOrDefault(
                                x => x.TemplateID == Foundation.Multisite.Templates.MainWebsite.ID
                             || x.TemplateID == Foundation.Multisite.Templates.MallWebsite.ID
                             || x.TemplateID == Foundation.Multisite.Templates.CommercialWebsite.ID);
            return (rootItem != null) ? IdHelper.NormalizeGuid(rootItem.ID) : string.Empty;
        }
    }
}
