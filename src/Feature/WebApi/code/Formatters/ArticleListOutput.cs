using Newtonsoft.Json;
using Sitecore.Feature.WebApi.Models;
using System.Collections.Generic;

namespace Sitecore.Feature.WebApi.Formatters
{
    public class ArticleListOutput : JsonOutput
    {
        [JsonProperty(Order = 0, PropertyName = "Articles")]
        public List<Article> Articles { get; set; }

        public ArticleListOutput(string status, List<Article> articles)
        {
            this.Status = status;
            this.Articles = articles;
        }
    }

    public class ArticleInfoListOutput : JsonOutput
    {
        [JsonProperty(Order = 0, PropertyName = "Articles")]
        public List<ArticleInfo> Articles { get; set; }

        public ArticleInfoListOutput(string status, List<ArticleInfo> articles)
        {
            this.Status = status;
            this.Articles = articles;
        }
    }
}