namespace Sitecore.Feature.Multisite
{
    using Sitecore.Data;

    public struct Templates
    {
        public struct FrasersRewards
        {
            public static ID ID => new ID("{875A246A-4F98-4F8B-BDA6-477C268B0607}");
        }
        public struct MallWebsite
        {
            public static ID ID => new ID("{6DE79C6F-136A-44CF-BC3B-A6CAAE03CB72}");
        }
        public struct CommercialWebsite
        {
            public static ID ID => new ID("{CD58A566-9A4C-4AFB-9EEC-96B416B54669}");
        }

        public struct Identity {
            public static ID ID => new ID("{FA8DE5B9-D5D8-40A7-866A-23AF4F5A9629}");
            public struct Fields
            {
                public static readonly ID SiteName = new ID("{D390B56F-6F6C-4DA7-832C-2ED4C44733E5}");
            }
        }
    }
}