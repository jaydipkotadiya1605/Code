namespace Sitecore.Feature.Errors.Pipelines
{
    using Sitecore.Feature.Errors.Utils;
    using Sitecore.Foundation.Abstractions.SitecoreContext;
    using Sitecore.Links;
    using Sitecore.Pipelines.HttpRequest;
    using System.Web;
    using System.Web.Mvc;

    public class HandleLayoutNotFound : HttpRequestProcessor
    {
        private readonly ISitecoreContext _sitecoreContext;

        public HandleLayoutNotFound() : this(DependencyResolver.Current.GetService<ISitecoreContext>())
        {
        }

        public HandleLayoutNotFound(ISitecoreContext sitecoreContext)
        {
            this._sitecoreContext = sitecoreContext;
        }

        public override void Process(HttpRequestArgs args)
        {
            if (UrlUtil.IsValidUrls(args.LocalPath))
                return;
            if (Context.Item == null)
            {
                return;
            }
            var currentItem = Context.Item;
            var homeItemPath = this._sitecoreContext.Site.StartPath;
            if (currentItem.Paths.Path.ToLower().Contains(homeItemPath.ToLower()) && currentItem.Visualization.Layout == null)
            {
                var itemNotFoundPageItemPath = this._sitecoreContext.ItemNotFoundPage;
                if (string.IsNullOrEmpty(itemNotFoundPageItemPath))
                {
                    return;
                }
                var pageNotFoundItem = UrlUtil.GetPageNotFoundItem(itemNotFoundPageItemPath);
                if (pageNotFoundItem != null)
                {
                    RedirectUtil.Do500Redirect(HttpContext.Current.Response, pageNotFoundItem);
                }
            }
        }
    }
}