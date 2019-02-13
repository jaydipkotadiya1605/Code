namespace Sitecore.Foundation.FrasersContent
{
    public static class Constants
    {
        public const string SpecialEventsName = "Special Event";
        public const int DedaultPageSize = 9;
        public const int DedaultPage = 0;
        public const int DedaultWidgetScores = 0;
        public const string LoadMore = "/Website/Load More";
        public const string LoadMoreText = "Load More";
        public const string NoDataFound = "/Website/No Data Found";
        public const string NoDataFoundText = "No Data Found";
        public const string DefaultLink = "/#";
        public const string StrPageIndexDefault = "1";
        public const int PageIndexDefault = 1;
        public const string ValueIsTrue = "1";
        public const string SitecoreMasterIndex = "sitecore_frasersrewards_master_index";
        public const string Score_FieldName = "score";
        public static readonly string MultipleMalls = "/Happenings/MultipleMalls";
        public static readonly string MultipleMallsText = "Multiple Malls";

        public struct BlogDictionary
        {
            public const string RelatedBlogTitle = "/Blogs/Articles You Might Like";
            public const string RelatedBlogTextDefault = "Articles You Might Like";
        }

        public struct WidgetSettingParameters
        {
            public static string LinkOfListPage => "Link to List Page";
            public static string NumberOfScores => "Number of Scores";
            public static string IsStorePage => "Is Store Page";
            public static string Categories => "Categories";
        }

        public struct PaggingParameters
        {
            public static string PageSize => "PageSize";
        }

        public struct QueryString
        {
            public static string Category => "category";
            public static string PageIndex => "pageIndex";
            public static string MallId => "mallId";
        }
    }
}