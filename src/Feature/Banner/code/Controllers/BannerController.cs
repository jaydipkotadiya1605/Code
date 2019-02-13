using Sitecore.Feature.Banner.Repositories;
using System.Web.Mvc;

namespace Sitecore.Feature.Banner.Controllers
{
    public class BannerController : Controller
    {
        private readonly IBannerRepository _bannerRepository;

        public BannerController(IBannerRepository bannerRepository)
        {
            this._bannerRepository = bannerRepository;
        }

        public ActionResult Banner()
        {
            var items = this._bannerRepository.GetBanner();
            return this.View("Banner", items);
        }
    }
}