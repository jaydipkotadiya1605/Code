using Sitecore.Diagnostics;
using Sitecore.Feature.WebApi.Formatters;
using Sitecore.Feature.WebApi.Services;
using System;
using System.Web.Http;

namespace Sitecore.Feature.WebApi.Controllers
{
    [RoutePrefix(Constants.ApiRouting.Root)]
    public class BannerController : BaseApiController
    {
        private readonly IBannerService bannerService;
        public BannerController(IBannerService bannerService)
        {
            this.bannerService = bannerService;
        }

        [Route(Constants.ApiRouting.BannerList)]
        public IHttpActionResult GetBanners()
        {
            try
            {
                this.GetPagingInfo(ref this.pageNo, ref this.pageSize);
                var listBanner = this.bannerService.GetBanners(string.Empty, this.pageNo, this.pageSize);
                var output = new BannerListOutput(Constants.ApiStatus.Success, listBanner);
                return this.JsonResult(output);
            }
            catch (Exception ex)
            {
                Log.Error($": GetBanners(). Error message: {ex.Message}", ex, this);
                var error = new JsonOutput(Constants.ApiStatus.Fail, ex.Message);
                return this.JsonResult(error);
            }
        }

        [Route(Constants.ApiRouting.BannerListForMall)]
        public IHttpActionResult GetBanners(string mall)
        {
            try
            {
                this.GetPagingInfo(ref this.pageNo, ref this.pageSize);
                var listBanner = this.bannerService.GetBannersBySitecode(mall, this.pageNo, this.pageSize);
                var output = new BannerListOutput(Constants.ApiStatus.Success, listBanner);
                return this.JsonResult(output);
            }
            catch (Exception ex)
            {
                Log.Error($": GetBanners({mall}). Error message: {ex.Message}", ex, this);
                var error = new JsonOutput(Constants.ApiStatus.Fail, ex.Message);
                return this.JsonResult(error);
            }
        }
    }
}
