namespace Sitecore.Feature.Navigation
{
    using Sitecore.Data;

    public struct Templates
    {
        public struct NavigationRoot
        {
            public static readonly ID ID = new ID("{7146E789-E6B5-4194-B307-AEAE359643DC}");
        }

        public struct LinkMenuItem
        {
            public static readonly ID ID = new ID("{32EECA74-3FCC-4055-87C6-990AB214C314}");

            public struct Fields
            {
                public static readonly ID Icon = new ID("{427DF8EA-6ADF-4889-9E10-2CEF1FF0C990}");
                public static readonly ID DividerBefore = new ID("{2D72ABD0-2583-4C18-A9D9-E1625232F956}");
            }
        }

        public struct NavigationLink
        {
            public static readonly ID ID = new ID("{8F0CBBF0-73D3-4CC1-97D3-45DF9A2FF358}");
            public struct Fields {
                public static readonly ID OrderOnMobile = new ID("{92750A07-EF30-40FE-83C8-308E01891298}");
            }
        }

        public struct TabPageMenu {
            public static readonly ID ID = new ID("{8A682C95-2D1B-48E6-B680-85C433727F3C}");
        }
    }
}