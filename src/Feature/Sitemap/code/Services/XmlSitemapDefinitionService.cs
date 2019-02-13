namespace Sitecore.Feature.Sitemap.Services
{
    using System.Collections.Generic;
    using System.Xml;
    using Sitecore.Data;
    using Sitecore.Feature.Sitemap.Models;
    using Sitecore.Foundation.DependencyInjection;
    using Sitecore.Xml;

    [Service(typeof(IXmlSitemapDefinitionService))]
    public class XmlSitemapDefinitionService : IXmlSitemapDefinitionService
    {
        public Dictionary<string, SitemapDefinitionModel> GetSitemapDefinitions()
        {
            var result = new Dictionary<string, SitemapDefinitionModel>();
            foreach (XmlNode node in Sitecore.Configuration.Factory.GetConfigNodes(Constants.SitemapNode))
            {
                var definition = new SitemapDefinitionModel()
                {
                    SiteName = XmlUtil.GetAttribute(Constants.SiteName, node),
                    EmbedLanguage = MainUtil.GetBool(XmlUtil.GetAttribute(Constants.EmbedLanguage, node), true)
                };

                definition.IncludedBaseTemplates = ParseXml2List(Constants.IncludeBaseTemplates, node);
                definition.IncludedTemplates = ParseXml2List(Constants.IncludeTemplates, node);
                definition.ExcludedItems = ParseXml2List(Constants.ExcludeItems, node);

                result.Add(definition.SiteName, definition);
            }

            return result;
        }

        private static List<ID> ParseXml2List(string childNodeName, XmlNode node, bool excluded = false)
        {
            var result = new List<ID>();
            var templates = XmlUtil.FindChildNode(childNodeName, node, excluded);
            if (templates != null)
            {
                foreach (XmlNode item in templates.ChildNodes)
                {
                    result.Add(ID.Parse(item.InnerText));
                }
            }
            return result;
        }
    }
}