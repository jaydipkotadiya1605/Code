namespace Sitecore.Feature.Navigation.Controllers
{
    using Sitecore.Feature.Navigation.Repositories;
    using Sitecore.Foundation.Abstractions.SitecoreContext;
    using Sitecore.Mvc.Presentation;
    using System.Web.Mvc;
    using System.Linq;
    using Sitecore.Foundation.SitecoreExtensions.Extensions;

    public class NavigationController : Controller
    {
        private readonly INavigationRepository _navigationRepository;

        public NavigationController(INavigationRepository navigationRepository, ISitecoreContext SitecoreContext)
        {
            this._navigationRepository = navigationRepository;
        }

        public ActionResult MainMenu()
        {
            var items = this._navigationRepository.GetMainMenu();
            return this.View("MainMenu", items);
        }

        public ActionResult MobileMainMenu()
        {
            var items = this._navigationRepository.GetMainMenu();
            return this.View("MobileMainMenu", items);
        }

        public ActionResult Breadcrumb()
        {
            var items = this._navigationRepository.GetBreadcrumb();
            return this.View("Breadcrumb", items);
        }

        public ActionResult TabMenu()
        {
            var items = this._navigationRepository.GetTabMenu(false);
            return this.View("TabMenu", items);
        }

        public ActionResult TabMenuSecondary()
        {
            var items = this._navigationRepository.GetTabMenu(true);
            return this.View("TabMenuSecondary", items);
        }

        public ActionResult TabFilter()
        {
            string categoryName = Request.QueryString[Constants.QueryString.Category] != null ? Request.QueryString[Constants.QueryString.Category].ToString() : string.Empty;
            string mallId = Request.QueryString[Constants.QueryString.MallId] != null ? Request.QueryString[Constants.QueryString.MallId].ToString() : string.Empty;
            var datasource = RenderingContext.Current.Rendering.DataSource;
            var items = this._navigationRepository.GetTabFilter(categoryName, datasource, mallId);
            return this.View("TabFilter", items);
        }

        public ActionResult MobileTabFilter()
        {
            string categoryName = Request.QueryString["category"] != null ? Request.QueryString["category"].ToString() : string.Empty;
            var datasource = RenderingContext.Current.Rendering.DataSource;
            var items = this._navigationRepository.GetTabFilter(categoryName, datasource);
            return this.View("MobileTabFilter", items);
        }

        public ActionResult HorizontalTabPages()
        {
            var items = this._navigationRepository.GetHorizontalTabPages();
            return this.View(items);
        }
    }
}