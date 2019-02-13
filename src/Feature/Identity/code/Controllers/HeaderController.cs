using Sitecore.Feature.Identity.Services;
using System.Web.Mvc;

namespace Sitecore.Feature.Identity.Controllers
{
    public class HeaderController : Controller
    {
        private readonly IHeaderService headerService;

        public HeaderController(IHeaderService headerService)
        {
            this.headerService = headerService;
        }

        public ActionResult SocialIcons()
        {
            return this.View(this.headerService.GetSocialIcons());
        }
        public ActionResult SocialIconsMobile()
        {
            return this.View(this.headerService.GetSocialIcons());
        }

        public ActionResult MainMenu()
        {
            return this.View(this.headerService.GetMainMenus());
        }
    }
}