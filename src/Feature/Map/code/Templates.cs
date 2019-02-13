namespace Sitecore.Feature.Map
{
    using Sitecore.Data;

    public struct Templates
    {
        public struct Contact
        {
            public static ID ID => new ID("{4C81C54A-4099-4859-8A4C-D429C9EAE510}");

            public struct Fields
            {
                public static ID Address => new ID("{CCA76F5B-F4A1-439E-9397-075F9CB6616E}");
                public static ID Tel => new ID("{427344BD-2182-4A75-B56A-7D3549BD1EB3}");
                public static ID WorkingHours => new ID("{732FE58F-7FA5-4278-850C-F06C99939B78}");
                public static ID CustomerService => new ID("{5A637948-2CAF-4062-94DA-B04AC741D46E}");
                public static ID Longitude => new ID("{2A98D119-1020-4854-AFAD-76E96A24331D}");
                public static ID Lattitude => new ID("{DA7DBF22-728D-4CBD-8573-6345BBB31861}");
            }

        }

        public struct SiteMetadata
        {
            public static ID ID => new ID("{CF38E914-9298-47CC-9205-210553E79F97}");

            public struct Fields
            {
                public static ID BrowserTitle => new ID("{235AE392-97AC-4822-BE38-837DA3E7724E}");
            }

        }

    }
}