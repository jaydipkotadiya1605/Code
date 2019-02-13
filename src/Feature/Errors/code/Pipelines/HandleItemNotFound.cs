namespace Sitecore.Feature.Errors.Pipelines
{
    using Sitecore.Data;
    using Sitecore.Diagnostics;
    using Sitecore.Foundation.Abstractions.SitecoreContext;
    using Sitecore.Pipelines.HttpRequest;
    using System.Web;
    using System.Web.Mvc;
    using Sitecore.Feature.Errors.Utils;
    using Sitecore.Links;

    public class HandleItemNotFound : HttpRequestProcessor
    {
        private readonly ISitecoreContext _sitecoreContext;

        public HandleItemNotFound() : this(DependencyResolver.Current.GetService<ISitecoreContext>())
        {
        }

        public HandleItemNotFound(ISitecoreContext sitecoreContext)
        {
            this._sitecoreContext = sitecoreContext;
        }

        public override void Process(HttpRequestArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            if (Utils.UrlUtil.IsValidUrls(args.LocalPath))
                return;
            if (Context.Item != null)
            {
                return;
            }

            var itemNotFoundPageItemPath = this._sitecoreContext.ItemNotFoundPage;
            if (string.IsNullOrWhiteSpace(itemNotFoundPageItemPath))
            {
                return;
            }

            var pageNotFoundItem = UrlUtil.GetPageNotFoundItem(itemNotFoundPageItemPath);
            RedirectUtil.Do404Redirect(HttpContext.Current.Response, pageNotFoundItem);

            Log.Warn("The 'Not Found Page: {0} shows no content when rendered!", itemNotFoundPageItemPath);
        }

    }
}