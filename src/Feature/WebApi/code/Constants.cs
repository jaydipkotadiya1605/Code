using Sitecore.Links;
using Sitecore.Resources.Media;

namespace Sitecore.Feature.WebApi
{
    public static class Constants
    {
        public static UrlOptions DefaultUrlOptions
        {
            get
            {
                var defaultOptions = LinkManager.GetDefaultUrlOptions();
                defaultOptions.AlwaysIncludeServerUrl = true;
                return defaultOptions;
            }
        }

        public static MediaUrlOptions DefaultMediaUrllOptions
        {
            get
            {
                var mediaUrlOptions = MediaUrlOptions.Empty;
                mediaUrlOptions.AlwaysIncludeServerUrl = true;
                return mediaUrlOptions;
            }
        }
        public struct ApiStatus
        {
            public const string Success = "success";
            public const string Fail = "fail";
        }

        public struct ApiParametter
        {
            public const string PageSize = "limit";
            public const string PageNo = "offset";
        }

        public struct ApiRouting
        {
            // API Root
            public const string Root = "feed";

            // Store
            public const string StoreList = "stores/list";
            public const string StoreListForMall = "stores/list/{mall}";
            public const string StoreDetail = "store/detail/{id}";

            // Banner
            public const string BannerList = "banner";
            public const string BannerListForMall = "banner/{mall}";

            // Article
            public const string ArticleList = "articles/list/{category}";
            public const string ArticleListForMall = "articles/list/{mall}/{category}";
            public const string ArticleDetail = "article/detail/{id}";
        }

        public const int DefaultPageSizeApi = 100;
        public const int DefaultPageNoApi = 0;
    }
}