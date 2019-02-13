namespace Sitecore.Foundation.Multisite
{
    using Sitecore.Data;

    public class Templates
    {
        public struct Site
        {
            public static readonly ID ID = new ID("{BB85C5C2-9F87-48CE-8012-AF67CF4F765D}");
        }

        public struct MainSiteSetting
        {
            public static readonly ID ID = new ID("{E28087EE-D697-4B24-BD04-57C3821C1EF0}");

            public struct Fields
            {
                // Multilist
                public static readonly ID MallSites = new ID("{A2B7BF42-7D1A-4BC8-8F0F-16E0D36D4AFB}");
            }
        }
        public struct MallSiteSetting
        {
            public static readonly ID ID = new ID("{C1918D32-A056-41F2-B49A-C6E7965E2CA1}");

            public struct Fields
            {
                // Droplink
                public static readonly ID MainSite = new ID("{2722BA68-BC77-4D4F-BC60-BB41F4B0C414}");
            }
        }

        public struct DatasourceConfiguration
        {
            public readonly static ID ID = new ID("{C82DC5FF-09EF-4403-96D3-3CAF377B8C5B}");

            public struct Fields
            {
                public static readonly ID DatasourceLocation = new ID("{5FE1CC43-F86C-459C-A379-CD75950D85AF}");
                public static readonly ID DatasourceTemplate = new ID("{498DD5B6-7DAE-44A7-9213-1D32596AD14F}");
            }
        }

        public struct SiteSettings
        {
            public static readonly ID ID = new ID("{BCCFEBEA-DCCB-48FE-9570-6503829EC03F}");
        }

        public struct RenderingOptions
        {
            public static readonly ID ID = new ID("{D1592226-3898-4CE2-B190-090FD5F84A4C}");

            public struct Fields
            {
                public static readonly ID DatasourceLocation = new ID("{B5B27AF1-25EF-405C-87CE-369B3A004016}");
                public static readonly ID DatasourceTemplate = new ID("{1A7C85E5-DC0B-490D-9187-BB1DBCB4C72F}");
            }
        }

        public struct MainSite
        {
            public static readonly ID ID = new ID("{2E7BEABD-4131-42E0-B551-C65BD8678A06}");

            // Check Box
            public struct Fields
            {
                public static readonly ID IsDisplayOnMainSite = new ID("{C1AF041D-C48B-4F5E-987A-E50CA0533C78}");
            }
        }
        public struct MallSite
        {
            public static readonly ID ID = new ID("{FCAFB699-D4A2-4193-BF6F-640E7CAF5A43}");

            // Multilist
            public struct Fields
            {
                public static readonly ID SiteDisplaySettings = new ID("{9518C26D-7A6E-4D02-BE50-E9E360C7957E}");
            }
        }

        public struct MainWebsite
        {
            public static readonly ID ID = new ID("{3C7F4F00-8981-4532-8EA8-8E6B05621911}");
        }

        public struct MallWebsite
        {
            public static readonly ID ID = new ID("{6DE79C6F-136A-44CF-BC3B-A6CAAE03CB72}");
        }

        public struct CommercialWebsite
        {
            public static readonly ID ID = new ID("{CD58A566-9A4C-4AFB-9EEC-96B416B54669}");
        }
    }
}