using Newtonsoft.Json;
using Sitecore.Feature.WebApi.Models;

namespace Sitecore.Feature.WebApi.Formatters
{
    public class StoreOutput : JsonOutput
    {
        [JsonProperty(Order = 0, PropertyName = "Store")]
        public Store Store { get; set; }

        public StoreOutput(string status, Store store)
        {
            this.Status = status;
            this.Store = store;
        }
    }
}