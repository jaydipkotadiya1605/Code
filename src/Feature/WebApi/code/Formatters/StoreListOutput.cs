using Newtonsoft.Json;
using Sitecore.Feature.WebApi.Models;
using System.Collections.Generic;

namespace Sitecore.Feature.WebApi.Formatters
{
    public class StoreListOutput : JsonOutput
    {
        [JsonProperty(Order = 0, PropertyName = "Stores")]
        public List<Store> Stores { get; set; }

        public StoreListOutput(string status, List<Store> stores)
        {
            this.Status = status;
            this.Stores = stores;
        }
    }

    public class StoreInfoListOutput : JsonOutput
    {
        [JsonProperty(Order = 0, PropertyName = "Stores")]
        public List<StoreInfo> Stores { get; set; }

        public StoreInfoListOutput(string status, List<StoreInfo> stores)
        {
            this.Status = status;
            this.Stores = stores;
        }
    }
}