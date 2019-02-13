namespace Sitecore.Feature.Teasers
{
    using Sitecore.Data;

    public struct Templates
    {
        public struct TeaserContent
        {
            public static readonly ID ID = new ID("{D76B0FA4-153C-4538-B2F7-B8F6AEE0BAA1}");
            public struct Fields
            {
                public static readonly ID TeaserTitle = new ID("{9C054856-F0E7-44D3-ADB5-5D8447FD5959}");
                public static readonly ID TeaserSummary = new ID("{6EB20209-0617-4A8B-A56D-8B9D3732A027}");
                public static readonly ID TeaserLink = new ID("{38AFAFEC-80BA-49EB-A17D-B3FC29659A2C}");
                public static readonly ID TeaserImage = new ID("{4A56287D-18AC-48DB-B825-8BE11400AF1A}");
            }
        }
    }
}