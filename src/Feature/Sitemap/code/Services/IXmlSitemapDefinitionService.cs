namespace Sitecore.Feature.Sitemap.Services
{
    using System.Collections.Generic;
    using Sitecore.Feature.Sitemap.Models;

    public interface IXmlSitemapDefinitionService
    {
        Dictionary<string, SitemapDefinitionModel> GetSitemapDefinitions();
    }
}