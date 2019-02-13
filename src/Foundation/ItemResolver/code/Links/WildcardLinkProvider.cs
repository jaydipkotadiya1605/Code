using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Foundation.ItemResolver.Extensions;
using Sitecore.Foundation.ItemResolver.Providers;
using Sitecore.Links;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net;

namespace Sitecore.Foundation.ItemResolver.Links
{
    /// <summary>
    /// Wildcard Link Provider
    /// </summary>
    public class WildcardLinkProvider : LinkProvider
    {
        /// <summary>
        /// Gets or sets the ignore for sites.
        /// </summary>
        private string[] IgnoreForSites { get; set; }

        /// <summary>
        /// Wildcard Url Token
        /// </summary>
        private string WildcardUrlToken { get; set; }

        /// <summary>
        /// Replace all non-alphanumeric characters with hyphens, ignoring slashes
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static string Normalize(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return path;
            }

            // Decode the path.
            path = WebUtility.HtmlDecode(path);
            path = MainUtil.DecodeName(path);

            var segments = path.Split(Constants.UrlSeparator, StringSplitOptions.RemoveEmptyEntries);
            var name = segments?.Length != 0 ? segments.Last() : null;
            if (!string.IsNullOrEmpty(name))
            {
                var replaced = ItemUtil.ProposeValidItemName(name).ToLower();
                path = path.Replace(name, replaced);
            }

            // Replace characters that Sitecore doesn't allow and lower case the rest.
            path = path.Replace(Constants.Blank, Constants.Hyphen);

            return path;
        }

        /// <summary>
        /// Initialize the LinkProvider
        /// </summary>
        /// <param name="name"></param>
        /// <param name="config"></param>
        public override void Initialize(string name, NameValueCollection config)
        {
            base.Initialize(name, config);

            // Load ignoreForSites attribute value
            var attribute = StringUtil.GetString((object)config[Constants.ConfigKey.IgnoreForSitesConfig], string.Empty);

            if (!string.IsNullOrEmpty(attribute))
            {
                this.IgnoreForSites = attribute.Replace(Constants.Blank, string.Empty).ToLower()
                                .Split(Constants.ListSeparator, StringSplitOptions.RemoveEmptyEntries);
            }

            // Load ignoreForSites attribute value
            this.WildcardUrlToken = StringUtil.GetString((object)config[Constants.ConfigKey.WildcardUrlTokenConfig], string.Empty);

            // Set Wildcard Url Token to defaut Wildcard Url Token
            if (string.IsNullOrEmpty(this.WildcardUrlToken))
            {
                this.WildcardUrlToken = Constants.Wildcard.UrlToken;
            }
        }

        /// <summary>
        /// Get a URL for the specified item
        /// </summary>
        /// <param name="item">Item</param>
        /// <param name="options">Url Option</param>
        /// <returns></returns>
        public override string GetItemUrl(Item item, UrlOptions options)
        {
            Assert.ArgumentNotNull(item, nameof(item));
            Assert.ArgumentNotNull(options, nameof(options));

            // Ignore custom linkprovider for sites listed in ignoreForSites attribute.
            // Only continue if we have a database so we're able to resolve the item
            if (this.IgnoreRequest(item, options))
            {
                // Return base GetItemUrl result if LinkProvider is ignored.
                return base.GetItemUrl(item, options);
            }

            string itemUrl;

            // Get route for link provider
            var routes = WildcardManager.Current?.GetWildcardRouteForLinkProvider(item, Context.Site);

            if (routes != null && routes.Any())
            {
                foreach (var route in routes)
                {
                    // Route is not existed or not valid to be processed
                    if (route == null || !route.IsValid)
                        continue;

                    // Get Wildcard Url
                    var wildcardUrl = this.GetItemUrl(route.WildcardNode, options);

                    // Append site name to real name if needed
                    var realName = route.IncludeSiteName ? $"{item.Name} {item.SiteName()}" : item.Name;

                    // Replace Wildcard with Real item
                    itemUrl = wildcardUrl.Replace(this.WildcardUrlToken, realName);

                    return Normalize(itemUrl);
                }
            }
           
            itemUrl = base.GetItemUrl(item, options);

            // Normalize Url
            return Normalize(itemUrl);
        }

        /// <summary>
        /// Default Url Options
        /// </summary>
        /// <returns></returns>
        public override UrlOptions GetDefaultUrlOptions()
        {
            var urlOptions = base.GetDefaultUrlOptions();
            urlOptions.SiteResolving = Settings.Rendering.SiteResolving;

            return urlOptions;
        }

        /// <summary>
        /// Indicates whether the request to this LinkProvider should be ignored and the base result must be returned.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        private bool IgnoreRequest(Item item, UrlOptions options)
        {
            // Ignore custom linkprovider for sites listed in ignoreForSites attribute.
            // Ignore for items that are not a child of the home item.
            // Only continue if we have a database so we're able to resolve the item
            return Context.Site == null 
                || Context.Database == null
                || Context.Database.Name.Equals(Constants.Sitecore.CoreDatabse, StringComparison.InvariantCultureIgnoreCase)
                || this.IgnoreForSites != null && this.IgnoreForSites.Contains(options.Site.Name.ToLower())
                || item.IsWilcard()
                || item.IsItemNotFound()
                || !item.Paths.FullPath.StartsWith(Constants.Sitecore.ContentRootPath, StringComparison.OrdinalIgnoreCase);
        }
    }
}