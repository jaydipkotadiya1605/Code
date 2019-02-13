using Sitecore.Feature.Banner.Models;

namespace Sitecore.Feature.Banner.Repositories
{
    public interface IBannerRepository
    {
        BannerItems GetBanner();
    }
}