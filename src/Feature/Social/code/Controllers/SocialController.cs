using Sitecore.Feature.Social.Services;
using System.Web.Mvc;

namespace Sitecore.Feature.Social.Controllers
{
    public class SocialController : Controller
    {
        private readonly ISocialService socialService;
        public SocialController(ISocialService socialService) {
            this.socialService = socialService;
        }
        public ActionResult OpenGraph()
        {
            return this.View(this.socialService.GetOpenGraphMetadata());
        }
    }
}