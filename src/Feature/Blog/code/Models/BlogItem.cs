using Sitecore.Data.Items;
using System;

namespace Sitecore.Feature.Blog.Models
{
    public class BlogItem
    {
        public Item Item { get; set; }
        public string Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Url { get; set; }
        public string Alt { get; set; }
        public DateTime? PostDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }
}