namespace Sitecore.Feature.Sitemap.Services
{
    using Sitecore.Feature.Sitemap.Models;
    using Sitecore.Sites;
    using System.Collections.Generic;

    public interface IXmlSitemapCollector
    {
        IEnumerable<SitemapModel> GetSitemapItems(SitemapDefinitionModel sitemapDefinition, SiteContext siteContext);
    }
}