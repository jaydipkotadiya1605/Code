namespace Sitecore.Feature.Sitemap.Services
{
    using System.Collections.Generic;
    using Sitecore.Feature.Sitemap.Models;

    public interface IXmlSitemapFileBuilder
    {
        string GenerateXml(IEnumerable<SitemapModel> sitemaps);
    }
}