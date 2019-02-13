namespace Sitecore.Feature.Sitemap.Models
{
    using System;

    public class SitemapModel
    {
        public string Url { get; set; }
        public DateTime LastModified { get; set; }
        public string ChangeFrequence { get; set; }
        public string Priority { get; set; }
    }
}