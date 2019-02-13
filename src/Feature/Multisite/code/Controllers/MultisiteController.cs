namespace Sitecore.Feature.Multisite.Controllers
{
    using Sitecore.Feature.Multisite.Services;
    using System.Web.Mvc;

    public class MultisiteController : Controller
    {
        private readonly IMultisiteService multisiteService;

        public MultisiteController(IMultisiteService multisiteService)
        {
            this.multisiteService = multisiteService;
        }

        public ActionResult Malls()
        {
            var result = this.multisiteService.GetAllMalls();
            return View(result);
        }

        public ActionResult SiteSwitcher()
        {
            var result = this.multisiteService.GetSites();
            return View(result);
        }
    }
}