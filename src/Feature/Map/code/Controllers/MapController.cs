using Sitecore.Feature.Map.Repositories;
using System.Web.Mvc;

namespace Sitecore.Feature.Map.Controllers
{
    public class MapController : Controller
    {
        private readonly ISiteRepository siteRepository;

        public MapController(ISiteRepository siteRepository)
        {
            this.siteRepository = siteRepository;
        }
        
        public ActionResult Location()
        {
            return this.View(this.siteRepository.GetShopInfo());
        }

        public ActionResult QuickInfo()
        {
            return this.View(this.siteRepository.GetShopInfo());
        }

        public ActionResult Map() {
            return this.View(this.siteRepository.GetMapLocation());
        }
    }
}