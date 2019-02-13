using System;
using System.Collections.Generic;

namespace Sitecore.Foundation.ItemResolver
{
    public static class Configuration
    {
        /// <summary>
        /// List ignored sites
        /// </summary>
        public static IList<string> IgnoreSites
        {
            get
            {
                if (_ignoreSites != null)
                {
                    return _ignoreSites;
                }

                var ignoreForSites = Sitecore.Configuration.Settings.GetSetting("IgnoreForSites", string.Empty) ?? string.Empty;
                _ignoreSites = string.IsNullOrWhiteSpace(ignoreForSites)
                    ? (IList<string>)new List<string>()
                    : ignoreForSites.ToLower().Split(Constants.ListSeparator, StringSplitOptions.RemoveEmptyEntries);

                return _ignoreSites;
            }
        }

        private static IList<string> _ignoreSites;
    }
}