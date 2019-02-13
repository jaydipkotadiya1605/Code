using Sitecore.Data;

namespace Sitecore.Foundation.Import
{
    public struct Templates
    {
        public struct Content {

            public static ID ID => new ID("{0DE95AE4-41AB-4D01-9EB0-67441B7C2450}");
        }

        public struct MallWebsite {
            public static ID ID => new ID("{6DE79C6F-136A-44CF-BC3B-A6CAAE03CB72}");
        }

        public struct LocalContentFolder {

            public static ID ID => new ID("{C85BE8FC-2D84-403E-8168-FF09FC74C878}");
            public const string DefaultName = "Local Content";
        }

        public struct BannerFolder
        {
            
            public static ID ID => new ID("{D668AAD6-791C-4DB0-AB9F-D78B182B569A}");

            public static readonly string DefaultName = "Banners";
        }
        public struct BlogFolder
        {
            public static ID ID => new ID("{68BF855C-205C-4578-BFBE-DA0F4B1FE2AA}");
            public  static readonly string DefaultName = "Blogs";
        }
        public struct ArticleFolder
        {
            public static ID ID => new ID("{24C25159-9565-4B90-8583-18CA41E1932B}");
            public static readonly string DefaultName = "Articles";

        }
        public struct EventFolder
        {
            public static ID ID => new ID("{9B65F886-900C-44C9-A2B0-BA609810EB6D}");
            public static readonly string DefaultName = "Events";
        }

        public struct ContentFolder
        {
            public static ID ID => new ID("{28F186D8-101C-4BCE-BAC6-39803D436537}");
            public static readonly string DefaultName = "Pages";
        }

        public struct StoreFolder
        {
            public static ID ID => new ID("{E3938AE8-0021-4A64-B646-CDCE6A04FA90}");

            public static readonly string DefaultName = "Stores";
        }

        public struct Store
        {
            public static ID ID => new ID("{5D406680-B088-46E8-9734-7370F2F6E9ED}");
        }

        public struct StoreCategories
        {
            public static ID GlobalRoot => new ID("{5415EFC9-AD12-410C-AFCD-59A644E1A7B7}");
            public static ID ID => new ID("{803DC367-7AED-4A95-B261-00FBDFDF1D0C}");
        }

        public struct Banner
        {
            public static ID ID => new ID("{F99D38F2-8B19-461B-89FD-3ED8262CA199}");
        }

        public struct Event
        {
            public static ID ID => new ID("{6620FA0A-49E6-4CAE-8872-395425075DC8}");
        }

        public struct Article
        {
            public static ID ID => new ID("{24F3889D-C63B-452D-B158-7601EB790305}");
        }

        public struct ArticleCatecory
        {
            public static ID ID => new ID("{0BD0A622-7CE0-47D9-B428-A1172171C3B8}");
            public static ID GlobalRoot => new ID("{C7C341F1-9FCB-4581-9745-6180A9EBA9C4}");
        }

        public struct LeasingResources
        {
            public static ID Atrium => new ID("{B6158A3E-616B-45BD-A8D6-8D21DBEC1591}");
            public static ID PushCart => new ID("{831D51F9-69B3-44B8-ABCC-07EC914ACDF9}");
            public static ID ShopSpace => new ID("{DC80AB7D-61BF-4B43-B9A0-0A29FEE73D77}");
        }

        public struct StoreOffer
        {
            public static ID AcceptsGiftCards => new ID("{80FE60BA-90EE-4B25-BFE3-8A7D3EA0F59D}");
            public static ID DigitalGiftCards => new ID("{EB90DD22-DDC8-4D83-860A-D0023000E5AE}");
            public static ID EarnFrasersPoints => new ID("{55F759E2-0FCC-4FAF-A46B-CDF16D08FCEF}");
            public static ID HalalCertified => new ID("{FE2265B5-E478-439B-ACA8-CE2223F6420A}");
        }

        public struct FrasersRewards
        {
            public static ID ID => new ID("{875A246A-4F98-4F8B-BDA6-477C268B0607}");
            public static readonly string MallSites = "Mall Sites";
        }

        public static readonly string MainSiteField = "Main Site";

        public struct BlogCategories
        {
            public static ID GlobalRoot => new ID("{EA99F9BC-728C-4E1E-AECF-46709BD2CC7D}");
            public static ID ID => new ID("{0E1D7B3E-2852-49BD-A7E6-6B2E5BA0B46F}");
        }
    }
}