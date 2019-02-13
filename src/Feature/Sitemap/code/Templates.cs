namespace Sitecore.Feature.Sitemap
{
    using Sitecore.Data;

    public struct Templates
    {
        public struct Sitemap
        {
            public static readonly ID Id = new ID("{D22123A6-A875-40D8-8D18-2100D9F117E9}");
        }

        public struct Robots
        {
            public static readonly ID Id = new ID("{5724A3EC-D896-4E88-B589-B59FBC240BCF}");

            public struct Fields
            {
                public static readonly ID Content = new ID("{375A10D1-51E9-4618-AB72-91D4B3E51537}");
            }
        }
    }
}