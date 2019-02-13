using Sitecore.Feature.WebApi.Models;
using System.Collections.Generic;

namespace Sitecore.Feature.WebApi.Services
{
    public interface IBannerService
    {
        List<Banner> GetBanners(string mallId, int pageNo, int pageSize);
        List<Banner> GetBannersBySitecode(string siteCode, int pageNo, int pageSize);
    }
}