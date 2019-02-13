using Sitecore.Diagnostics;
using Sitecore.Feature.Errors.Utils;
using Sitecore.Foundation.Abstractions.SitecoreContext;
using Sitecore.Mvc.Pipelines.MvcEvents.Exception;
using Sitecore.Web;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.Feature.Errors.Pipelines
{
    public class HandleMvcException : ExceptionProcessor
    {
        private readonly ISitecoreContext _sitecoreContext;

        public HandleMvcException() : this(DependencyResolver.Current.GetService<ISitecoreContext>())
        {
        }

        public HandleMvcException(ISitecoreContext sitecoreContext)
        {
            this._sitecoreContext = sitecoreContext;
        }

        public override void Process(ExceptionArgs args)
        {
            var context = args.ExceptionContext;
            var httpContext = context.HttpContext;
            var exception = context.Exception;
            
            if (context.ExceptionHandled || httpContext == null || exception == null)
            {
                return;
            }
            Log.Error(string.Format("There was an error in {0} : {1}", Context.Site.Name, exception), this);

            // Return a 500 status code and execute the custom error page.
            HttpContext.Current.Server.ClearError();
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.TrySkipIisCustomErrors = true;
            HttpContext.Current.Response.StatusCode = 500;
            HttpContext.Current.Response.StatusDescription = "Internal Exception";

            var serverErrorPage = _sitecoreContext.ServerErrorPage;
            var targetUrl = UrlUtil.GetPageNotFoundItem(serverErrorPage);
            string content = WebUtil.ExecuteWebPage(targetUrl);

            // write out 500 page html content
            HttpContext.Current.Response.Write(content);
            HttpContext.Current.Response.Redirect(targetUrl);
            HttpContext.Current.Response.End();

        }
    }
}