using Sitecore.Data.Items;
using System;

namespace Sitecore.Feature.Article.Models
{
    public class ArticleItem
    {
        public Item Item { get; set; }
        public string Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Alt { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}