namespace Sitecore.Feature.Social
{
    using Sitecore.Data;

    public struct Templates
    {
        public struct OpenGraph
        {
            public static readonly ID ID = new ID("{BDD24F35-05E8-4466-8798-7D3DD6A6C991}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{0EE2F208-1FEE-42FC-8051-6696D49A92BF}");
                public static readonly string TitleName = "Title";

                public static readonly ID Description = new ID("{A12D5346-87EE-484D-83C5-F1E8E1B99666}");
                public static readonly string DescriptionName = "Summary";

                public static readonly ID Image = new ID("{11F41661-E5FE-44E7-B8DA-7CFF2D39B4B2}");
                public static readonly string ImageName = "Thumbnail";

                public static readonly ID Type = new ID("{898A8C92-262E-44AD-9F8D-83D7263EA5B1}");
            }
        }
    }
}