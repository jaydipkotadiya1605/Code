using System.Collections.Generic;

namespace Sitecore.Feature.Sitemap.Models
{
    using Sitecore.Data;

    public class SitemapDefinitionModel
    {
        public List<ID> IncludedBaseTemplates { get; set; }
        public List<ID> IncludedTemplates { get; set; }
        public List<ID> ExcludedItems { get; set; }
        public bool EmbedLanguage { get; set; }
        public string SiteName { get; set; }
    }
}