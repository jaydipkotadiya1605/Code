using Sitecore.Feature.Map.Models;

namespace Sitecore.Feature.Map.Repositories
{
    public interface ISiteRepository
    {
        ContactInfo GetShopInfo();
        MapLocation GetMapLocation();
    }
}