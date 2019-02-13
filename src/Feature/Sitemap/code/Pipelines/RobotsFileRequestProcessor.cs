namespace Sitecore.Feature.Sitemap.Pipelines
{
    using Sitecore.Feature.Sitemap.ViewModelBuilders;
    using Sitecore.Foundation.Abstractions.SitecoreContext;
    using Sitecore.Foundation.Multisite.Providers;
    using Sitecore.Pipelines.HttpRequest;
    using System.Web;
    using System.Web.Mvc;

    public class RobotsFileRequestProcessor : HttpRequestProcessor
    {
        private readonly ISitecoreContext _sitecoreContext;
        private readonly IRobotsViewModelBuilder _robotsViewModelBuilder;
        private readonly ISiteSettingsProvider _siteSettingsProvider;

        public RobotsFileRequestProcessor() : this(
            DependencyResolver.Current.GetService<ISitecoreContext>(),
            DependencyResolver.Current.GetService<ISiteSettingsProvider>(),
            DependencyResolver.Current.GetService<IRobotsViewModelBuilder>()
            )
        {
        }

        public RobotsFileRequestProcessor(
            ISitecoreContext sitecoreContext,
            ISiteSettingsProvider siteSettingsProvider,
            IRobotsViewModelBuilder robotsViewModelBuilder
            )
        {
            this._sitecoreContext = sitecoreContext;
            this._siteSettingsProvider = siteSettingsProvider;
            this._robotsViewModelBuilder = robotsViewModelBuilder;
        }

        public override void Process(HttpRequestArgs args)
        {
            var context = HttpContext.Current;
            var requestUrl = context?.Request.Url.AbsolutePath;

            if (string.IsNullOrEmpty(requestUrl) || requestUrl.ToLower() != "/robots.txt")
                return;

            context.Response.Clear();
            context.Response.AddHeader("Content-Disposition", "attachment;filename=robots.txt");
            context.Response.ContentType = "text/plain";

            var rootItem = this._sitecoreContext.SiteRoot;
            var robotsItem = this._siteSettingsProvider.GetSetting(rootItem, Templates.Robots.Id);
            if (robotsItem == null)
            {
                context.Response.StatusCode = 404;
                return;
            }

            var robotsContent = this._robotsViewModelBuilder.GetRobotsContent(robotsItem);

            context.Response.Write(robotsContent);
            context.Response.End();

            args.AbortPipeline();
        }
    }
}