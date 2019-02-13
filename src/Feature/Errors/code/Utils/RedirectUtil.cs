namespace Sitecore.Feature.Errors.Utils
{
    using System.Net;
    using Sitecore.Web;
    using System.Web;

    public static class RedirectUtil
    {
        public static void Do500Redirect(HttpResponse response, string targetUrl)
        {
            // set Response params
            response.TrySkipIisCustomErrors = true;
            response.StatusCode = 500;
            response.StatusDescription = "Internal Exception";
            string content = WebUtil.ExecuteWebPage(targetUrl);
            // write out 500 page html content
            response.Write(content);
            response.End();
        }

        public static void Do404Redirect(HttpResponse response, string targetUrl)
        {
            response.TrySkipIisCustomErrors = true;
            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.StatusDescription = "Item not found";
            string content = WebUtil.ExecuteWebPage(targetUrl);
            response.Write(content);
            response.End();
        }
    }
}