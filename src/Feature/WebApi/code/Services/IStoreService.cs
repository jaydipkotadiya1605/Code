using Sitecore.Feature.WebApi.Models;
using System.Collections.Generic;

namespace Sitecore.Feature.WebApi.Services
{
    public interface IStoreService
    {
        List<StoreInfo> GetStores(string siteCode, int pageNo, int pageSize);
    }
}