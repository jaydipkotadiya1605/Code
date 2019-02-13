using Newtonsoft.Json;
using Sitecore.Feature.WebApi.Models;

namespace Sitecore.Feature.WebApi.Formatters
{
    public class ArticleOutput : JsonOutput
    {
        [JsonProperty(Order = 0, PropertyName = "Article")]
        public Article Article { get; set; }

        public ArticleOutput(string status, Article article)
        {
            this.Status = status;
            this.Article = article;
        }
    }
}