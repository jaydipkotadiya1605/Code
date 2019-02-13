namespace Sitecore.Feature.Store.Controllers
{
    using Sitecore.Feature.Store.Services;
    using System.Web.Mvc;
    using Sitecore.Feature.Store.Models;
    using Sitecore.Foundation.Abstractions.SitecoreContext;
    using Sitecore.Foundation.Alerts.Exceptions;
    using Sitecore.Foundation.SitecoreExtensions.Extensions;
    using Sitecore.Foundation.SitecoreExtensions.Repositories;
    using Sitecore.Mvc.Presentation;

    public class StoreController : Controller
    {
        private readonly IStoreService storeService;
        private readonly IRenderingPropertiesRepository renderingPropertiesRepository;
        private readonly ISitecoreContext sitecoreContext;

        public StoreController(
            IStoreService storeService,
            IRenderingPropertiesRepository renderingPropertiesRepository,
            ISitecoreContext sitecoreContext)
        {
            this.storeService = storeService;
            this.renderingPropertiesRepository = renderingPropertiesRepository;
            this.sitecoreContext = sitecoreContext;
        }

        public ActionResult AlphabetFilters()
        {
            var results = this.storeService.GetAlphabetFilters();
            return View(results);
        }

        public ActionResult StoreCategories()
        {
            var results = this.storeService.GetAllCategories();
            return View(results);
        }

        public ActionResult Wing(string mallId = "")
        {
            string selectedMall = Request.QueryString[Constants.QueryString.MallId] != null ? Request.QueryString[Constants.QueryString.MallId] : mallId;
            var results = this.storeService.GetAllWings(selectedMall);
            return View(results);
        }

        public ActionResult StoreOffers(string mallId = "")
        {
            string selectedMall = Request.QueryString[Constants.QueryString.MallId] != null ? Request.QueryString[Constants.QueryString.MallId] : mallId;
            var results = this.storeService.GetAllStoreOffers(selectedMall);
            return View(results);
        }

        public ActionResult SearchByAlphabet() => this.ExecuteSearch("SearchByAlphabet");

        public ActionResult SearchByKeyword() => this.ExecuteSearch("SearchByKeyword");

        public ActionResult StoreQuickFinder()
        {
            var searchResultPageUrl = this.storeService.GetSearchResultPage(RenderingContext.Current.Rendering);
            return View(searchResultPageUrl);
        }

        public ActionResult RelatedStores()
        {
            var item = this.sitecoreContext.DataSourceOrSelf;
            if (!item.IsDerived(Foundation.FrasersContent.Templates.Store.ID))
            {
                throw new InvalidDataSourceItemException($"Item should be not null and derived from {nameof(Foundation.FrasersContent.Templates.Store)} {Foundation.FrasersContent.Templates.Store.ID} template");
            }

            var mallSite = item.GetAncestorOrSelfOfTemplate(Foundation.Multisite.Templates.Site.ID);
            var criteria = this.renderingPropertiesRepository.Get<RelatedStoresSearchCriteria>(RenderingContext.Current.Rendering);
            criteria.CurrentMallId = mallSite?.ID;
            criteria.CurrentStoreId = item.ID;

            var results = this.storeService.GetLatestStores(criteria);
            return this.View(results);
        }

        private ActionResult ExecuteSearch(string viewName)
        {
            var renderingId = RenderingContext.CurrentOrNull.Rendering.UniqueId.ToString("N");
            var defaultConfiguration = this.renderingPropertiesRepository.Get<SearchCriteria>(RenderingContext.Current.Rendering);
            var criteria = this.renderingPropertiesRepository.GetExt<SearchCriteria>(RenderingContext.Current.Rendering);
            if (string.IsNullOrEmpty(criteria.RenderingId) || !criteria.RenderingId.Equals(renderingId))
            {
                criteria.PageIndex = defaultConfiguration.PageIndex;
                criteria.PageSize = defaultConfiguration.PageSize;
            }

            var results = this.storeService.Search(criteria);
            results.RenderingId = renderingId;

            return this.View(viewName, results);
        }
    }
}