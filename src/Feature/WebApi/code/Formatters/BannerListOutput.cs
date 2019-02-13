using Newtonsoft.Json;
using Sitecore.Feature.WebApi.Models;
using System.Collections.Generic;

namespace Sitecore.Feature.WebApi.Formatters
{
    public class BannerListOutput : JsonOutput
    {
        [JsonProperty(Order = 0, PropertyName = "Banners")]
        public List<Banner> Banners { get; set; }

        public BannerListOutput(string status, List<Banner> banners)
        {
            this.Status = status;
            this.Banners = banners;
        }
    }
}