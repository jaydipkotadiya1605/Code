using Newtonsoft.Json;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Foundation.SitecoreExtensions.Extensions;

namespace Sitecore.Feature.WebApi.Models
{
    public class Article
    {
        [JsonProperty(Order = 1, PropertyName = "id")]
        public ID Id { get; set; }
        [JsonProperty(Order = 2, PropertyName = "title")]
        public string Title { get; set; }
        [JsonProperty(Order = 3, PropertyName = "thumbnail")]
        public string Thumbnail { get; set; }
        [JsonProperty(Order = 4, PropertyName = "summary")]
        public string Summary { get; set; }
        [JsonProperty(Order = 5, PropertyName = "HTML")]
        public string Html { get; set; }
        [JsonProperty(Order = 6, PropertyName = "share_link")]
        public string ShareLink { get; set; }
        
        public Article()
        {
            this.Id = null;
            this.Title = null;
            this.Thumbnail = null;
            this.Summary = null;
            this.Html = null;
            this.ShareLink = null;
        }

        public Article(Item item)
        {
            if (item == null)
            {
                return;
            }

            this.Id = item.ID;
            this.Title = item.Fields[Templates.Article.Title].Value;
            this.Summary = item.Fields[Templates.Article.Summary].Value;

            // Get Thumbnail
            this.Thumbnail = item.ImageUrl(Templates.Article.Thumbnail);

            this.Html = item.Fields[Templates.Article.Description].Value;

            // Get ShareLink
            this.ShareLink = item.Url(Constants.DefaultUrlOptions);
        }
    }

    public class ArticleInfo
    {
        [JsonProperty(Order = 1, PropertyName = "Id")]
        public ID Id { get; set; }
        [JsonProperty(Order = 2, PropertyName = "title")]
        public string Title { get; set; }
        [JsonProperty(Order = 3, PropertyName = "thumbnail")]
        public string Thumbnail { get; set; }
        [JsonProperty(Order = 4, PropertyName = "summary")]
        public string Summary { get; set; }
        [JsonProperty(Order = 5, PropertyName = "share_link")]
        public string ShareLink { get; set; }

        public ArticleInfo()
        {
            this.Id = null;
            this.Title = null;
            this.Thumbnail = null;
            this.Summary = null;
            this.ShareLink = null;
        }

        public ArticleInfo(Item item)
        {
            if (item == null)
            {
                return;
            }

            this.Id = item.ID;
            this.Title = item.Fields[Templates.Article.Title].Value;
            this.Summary = item.Fields[Templates.Article.Summary].Value;

            // Get Thumbnail
            this.Thumbnail = item.ImageUrl(Templates.Article.Thumbnail);

            // Get ShareLink
            this.ShareLink = item.Url(Constants.DefaultUrlOptions);
        }
    }
}