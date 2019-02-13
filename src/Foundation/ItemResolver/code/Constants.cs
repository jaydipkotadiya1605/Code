namespace Sitecore.Foundation.ItemResolver
{
    public static class Constants
    {
        public static readonly string Slash = "/";
        public static readonly string Hyphen = "-";
        public static readonly string Blank = " ";

        public static string[] ListSeparator { get; } = { "|" };
        public static string[] UrlSeparator { get; } = { "/" };

        public static readonly string CustomUrlResloverCacheKey = "custom::ItemResolved";
        public static string LocalContentPath => "/local-content/";

        public struct ConfigKey
        {
            public static readonly string ItemResolverSettingsConfig = "itemResolverSettings"; 
            public static readonly string IgnoreForSitesConfig = "ignoreForSites";
            public static readonly string WildcardUrlTokenConfig = "wildcardUrlToken";
            public static readonly string WildcardManager = "wildcardManager";
            public static readonly string WildcardItemResolver = "wildcardItemResolver";
        }

        public struct Sitecore
        {
            public static readonly string DefaultDomain = "sitecore";
            public static readonly string CoreDatabse = "core";
            public static string ContentRootPath => "/sitecore/content";
        }

        public struct Wildcard
        {
            public static readonly string UrlToken = ",-w-,";
            public static readonly string Node = "*";
        }
    }
}