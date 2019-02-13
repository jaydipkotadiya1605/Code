namespace Sitecore.Feature.Store.Services
{
    using Sitecore.Feature.Store.Models;
    using System.Collections.Generic;

    public interface IStoreService
    {
        IEnumerable<AlphabetFilterViewModel> GetAlphabetFilters();

        IEnumerable<StoreCategoryViewModel> GetAllCategories();

        IEnumerable<WingViewModel> GetAllWings(string mallId);

        IEnumerable<StoreOfferViewModel> GetAllStoreOffers(string mallId);

        StoreQuicFinderViewModel GetSearchResultPage(Mvc.Presentation.Rendering rendering);

        StoreSearchResultViewModel Search(SearchCriteria criteria);

        RelatedStoresViewModel GetLatestStores(RelatedStoresSearchCriteria criteria);
    }
}