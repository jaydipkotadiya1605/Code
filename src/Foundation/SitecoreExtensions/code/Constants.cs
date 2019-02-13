namespace Sitecore.Foundation.SitecoreExtensions
{
    using Sitecore.Data;
    public struct Constants
    {
        public static readonly string PathDelimiter = "/";
        public static readonly char PathDelimiterCharacter = '/';

        public struct DynamicPlaceholdersLayoutParameters
        {
            public static string UseStaticPlaceholderNames => "UseStaticPlaceholderNames";
        }

        public static readonly ID LinkedOffers = new ID("{AFA51595-7CFB-495D-9173-89CDCE18787A}");

        public static readonly ID HideInSitesID = new ID("{67111C0C-0AE5-4C27-B059-94609EB52B0F}");

        public static readonly ID MainWebsiteID = new ID("{3C7F4F00-8981-4532-8EA8-8E6B05621911}");

        public static readonly ID MallWebsiteID = new ID("{6DE79C6F-136A-44CF-BC3B-A6CAAE03CB72}");

        public static readonly ID CommercialWebsiteID = new ID("{CD58A566-9A4C-4AFB-9EEC-96B416B54669}");

    }
}