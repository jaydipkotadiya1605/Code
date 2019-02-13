namespace Sitecore.Feature.Search.Controllers
{
    using Sitecore.Feature.Search.Services;
    using Sitecore.Mvc.Presentation;
    using System.Web.Mvc;
    using Sitecore.Feature.Search.Models;
    using Sitecore.Foundation.SitecoreExtensions.Repositories;

    public class SearchController : Controller
    {
        private readonly ISearchService searchService;
        private readonly IRenderingPropertiesRepository renderingPropertiesRepository;

        public SearchController(ISearchService searchService, IRenderingPropertiesRepository renderingPropertiesRepository)
        {
            this.searchService = searchService;
            this.renderingPropertiesRepository = renderingPropertiesRepository;
        }

        public ActionResult LatestPosts()
        {
            var criteria = this.renderingPropertiesRepository.Get<FraserRewardsFilterModel>(RenderingContext.Current.Rendering);
            var model = this.searchService.GetLatestFraserRewards(criteria);
            var renderingId = RenderingContext.CurrentOrNull.Rendering.UniqueId.ToString("N");
            model.RenderingId = renderingId;
            return this.View(model);
        }

        public ActionResult LatestFraserRewards()
        {
            var criteria = this.renderingPropertiesRepository.GetExt<FraserRewardsFilterModel>(RenderingContext.Current.Rendering);
            var model = this.searchService.GetLatestFraserRewards(criteria);
            var renderingId = RenderingContext.CurrentOrNull.Rendering.UniqueId.ToString("N");
            model.RenderingId = renderingId;
            return this.View(model);
        }

        public ActionResult SearchHeader()
        {
            var model = this.searchService.GetSearchHeader(RenderingContext.Current.Rendering);
            return this.View(model);
        }

        public ActionResult SearchOverlay()
        {
            var model = this.searchService.GetSearchOverlay(RenderingContext.Current.Rendering);
            return this.View(model);
        }

        public ActionResult SearchByKeyword()
        {
            var renderingId = RenderingContext.CurrentOrNull.Rendering.UniqueId.ToString("N");
            var defaultConfiguration = this.renderingPropertiesRepository.Get<FraserRewardsFilterModel>(RenderingContext.Current.Rendering);
            var criteria = this.renderingPropertiesRepository.GetExt<FraserRewardsFilterModel>(RenderingContext.Current.Rendering);
            if (string.IsNullOrEmpty(criteria.RenderingId) || !criteria.RenderingId.Equals(renderingId))
            {
                criteria.PageIndex = defaultConfiguration.PageIndex;
                criteria.PageSize = defaultConfiguration.PageSize;
            }
            var model = this.searchService.Search(criteria);
            model.RenderingId = renderingId;
            return this.View(model);
        }
    }
}