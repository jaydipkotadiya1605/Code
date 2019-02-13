namespace Sitecore.Feature.Search
{
    using Sitecore.Data;

    public struct Templates
    {
        public struct SearchHeader
        {
            public static readonly ID ID = new ID("{03F05F54-51F0-47DE-8DD2-A07A61A6577B}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{13AC1397-0FF5-49E1-92EB-A903BD1798C2}");
                public static readonly ID Subtitle = new ID("{1FBEC6C4-F3BC-4B00-A4B8-3AD05206B349}");
                public static readonly ID PlaceholderText = new ID("{5A21AE5B-DA72-46DC-AB15-0DA32A86B424}");
                public static readonly ID ButtonSearchText = new ID("{D8365986-17A9-4257-8B04-ACEB5EFB36BF}");
            }
        }

        public struct SearchResultPageParameter
        {
            public static readonly ID ID = new ID("{54A85215-2834-44B6-8229-A5018A4C222D}");
            public struct Fields
            {
                public static readonly ID SearchResultPage = new ID("{A4152E3D-E084-4D73-9E74-301B246E86EB}");
            }
        }
        public struct SearchSettings
        {
            public static readonly ID ID = new ID("{FD48B81C-66F7-492D-A894-92B61E38F3A2}");
            public struct Fields
            {
                public static readonly ID OverlaySearchResultPage = new ID("{93678E0D-36E4-4257-983D-B3C08355412B}");
            }
        }
    }
}