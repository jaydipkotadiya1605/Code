using System.Linq;

namespace Sitecore.Feature.Errors.Utils
{
    using Sitecore.Links;

    public static class UrlUtil
    {
        public static bool IsValidUrls(string url)
        {
            return Constants.ExcludedPaths.Any(url.StartsWith);
        }

        public static string GetPageNotFoundItem(string itemNotFoundPageItemPath)
        {
            var item = Context.Database.GetItem(itemNotFoundPageItemPath);
            var options = LinkManager.GetDefaultUrlOptions();
            options.AlwaysIncludeServerUrl = false;
            options.AddAspxExtension = false;
            return LinkManager.GetItemUrl(item, options);
        }
    }
}