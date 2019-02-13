namespace Sitecore.Feature.Sitemap.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Sitecore.Feature.Sitemap.Models;
    using Sitecore.Foundation.DependencyInjection;
    using System;

    [Service(typeof(IXmlSitemapFileBuilder))]
    public class XmlSitemapFileBuilder : IXmlSitemapFileBuilder
    {
        public string GenerateXml(IEnumerable<SitemapModel> sitemaps)
        {
            var sb = new StringBuilder();

            sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sb.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd\">");
            sb.AppendLine(string.Concat(sitemaps.Select(CreateUrlElement)));
            sb.AppendLine("</urlset>");

            return sb.ToString();
        }

        private static string CreateUrlElement(SitemapModel sitemapItem)
        {
            var lastMod = string.Empty;
            if (sitemapItem.LastModified != DateTime.MaxValue && sitemapItem.LastModified != DateTime.MinValue)
            {
                lastMod = $"<lastmod>{sitemapItem.LastModified:yyyy-MM-dd}</lastmod>";
            }

            return $"<url><loc>{sitemapItem.Url}</loc>{lastMod}</url>";
        }
    }
}