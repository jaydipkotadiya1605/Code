namespace Sitecore.Feature.Errors
{
    using Sitecore.Data;

    public struct Templates
    {
        public struct ItemNotFound
        {
            public static readonly ID Id = new ID("{F01288A2-1044-4C2C-BBAD-4426884CB97B}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{39CA1862-E2F6-40BF-B732-373B7FEB2E85}");
                public static readonly ID Subtitle = new ID("{6DEE338E-B839-4A00-9C88-2FF563C976F9}");
            }
        }

        public struct Error
        {
            public static readonly ID Id = new ID("{BDF950BA-A592-4CA6-9BCE-971E234E6E51}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{15B60B1D-A7CF-4009-BA7C-30DBA7F24F6F}");
                public static readonly ID Subtitle = new ID("{DC899263-B6C9-4071-922A-3358B319E0CF}");
            }
        }
    }
}