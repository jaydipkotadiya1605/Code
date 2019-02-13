namespace Sitecore.Feature.Sitemap.Pipelines
{
    using Sitecore.Feature.Sitemap.Services;
    using Sitecore.Pipelines.HttpRequest;
    using Sitecore.Sites;
    using Sitecore.Web;
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    public class XmlSitemapFileRequestProcessor : HttpRequestProcessor
    {
        private readonly IXmlSitemapFileBuilder _builder;
        private readonly IXmlSitemapCollector _sitemapCollector;
        private readonly IXmlSitemapDefinitionService _sitemapDefinitionService;

        public XmlSitemapFileRequestProcessor() : this(
            DependencyResolver.Current.GetService<IXmlSitemapFileBuilder>(),
            DependencyResolver.Current.GetService<IXmlSitemapCollector>(),
            DependencyResolver.Current.GetService<IXmlSitemapDefinitionService>())
        {
        }

        public XmlSitemapFileRequestProcessor(
            IXmlSitemapFileBuilder builder,
            IXmlSitemapCollector sitemapCollector,
            IXmlSitemapDefinitionService sitemapDefinitionService)
        {
            this._builder = builder;
            this._sitemapCollector = sitemapCollector;
            this._sitemapDefinitionService = sitemapDefinitionService;
        }

        public override void Process(HttpRequestArgs args)
        {
            var context = HttpContext.Current;
            var requestUrl = context?.Request.Url.AbsolutePath;

            if (string.IsNullOrEmpty(requestUrl) || requestUrl.ToLower() != "/sitemap.xml")
                return;

            context.Response.Clear();
            context.Response.AddHeader("Content-Disposition", "attachment;filename=sitemap.xml");
            context.Response.ContentType = "text/xml";

            var siteInfo = this.GetSiteInfo();
            if (siteInfo == null || (siteInfo.Port > 0 && siteInfo.Port != context.Request.Url.Port))
            {
                context.Response.StatusCode = 404;
                return;
            }

            var sitemapDefinition = this._sitemapDefinitionService.GetSitemapDefinitions()
                .FirstOrDefault(x => x.Key.Equals(siteInfo.Name, StringComparison.InvariantCultureIgnoreCase))
                .Value;

            if (sitemapDefinition == null)
            {
                context.Response.StatusCode = 404;
                return;
            }

            var urlList = this._sitemapCollector.GetSitemapItems(sitemapDefinition, SiteContext.Current);

            var xml = this._builder.GenerateXml(urlList);

            context.Response.Write(xml);
            context.Response.End();

            args.AbortPipeline();
        }

        private SiteInfo GetSiteInfo()
        {
            return Configuration.Factory.GetSiteInfoList()
                .FirstOrDefault(i => i.HostName != null &&
                                     i.HostName.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries)
                                         .Any(n => string.Equals(n, HttpContext.Current.Request.Url.Host, StringComparison.CurrentCultureIgnoreCase)));
        }
    }
}