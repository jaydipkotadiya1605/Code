using Sitecore.Data;

namespace Sitecore.Foundation.ItemResolver
{
    public static class Templates
    {
        public struct ItemResolverSettings
        {
            public static readonly ID ID = new ID("{3BBEFAB1-43B0-40D2-BF07-570B873BF68F}");
        }

        public struct ItemResolverRule
        {
            public static readonly ID ID = new ID("{54329509-EABF-48CC-90E1-8A7BEF3F94CE}");

            public struct Fields
            {
                public static readonly ID WildcardNode = new ID("{11E1F887-DC46-4A13-83A2-22642CBCFD10}");
                public static readonly ID DataTemplate = new ID("{42EFCDE2-80DD-4C46-AE8A-27B7DC15DF76}");
                public static readonly ID SearchRootNode = new ID("{E1F8D80F-4453-4E93-8B13-B06707036628}");
                public static readonly ID IncludeSiteName = new ID("{981DC08B-B585-4EB1-9507-766998633625}");
            }
        }

        public struct Wildcard
        {
            public static readonly ID ID = new ID("{5B342837-23E9-4787-AC59-65FE377E52AE}");
        }

        public struct ItemNotFoundPage
        {
            public static readonly ID ID = new ID("{B7E7BC8A-6678-4ED0-B772-42946BA56F01}");
        }

        public struct NavigationLink
        {
            public static readonly ID ID = new ID("{8F0CBBF0-73D3-4CC1-97D3-45DF9A2FF358}");

            public struct Fields
            {
                public static readonly ID Link = new ID("{E81C5D1D-E1DE-47E7-A5C1-D3C77DB7E75B}");
            }
        }

        public struct MainSite
        {
            public static readonly ID ID = new ID("{3C7F4F00-8981-4532-8EA8-8E6B05621911}");
        }

        public struct MallSite
        {
            public static readonly ID ID = new ID("{6DE79C6F-136A-44CF-BC3B-A6CAAE03CB72}");
        }
    }
}