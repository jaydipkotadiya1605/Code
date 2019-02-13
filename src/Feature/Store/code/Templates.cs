namespace Sitecore.Feature.Store
{
    using Sitecore.Data;

    public struct Templates
    {
        public struct Alphabet
        {
            public static readonly ID ID = new ID("{5D1F33F2-D893-4EC6-8177-8C284C9740DA}");

            public struct Fields
            {
                public static readonly ID Text = new ID("{8F9C4C04-5D54-4186-AAB6-E95F894FC642}");
                public static readonly ID Keyword = new ID("{79EBDB01-F448-4955-B62D-2411E20C8E11}");
            }
        }

        public struct DisplayOption
        {
            public static readonly ID ID = new ID("{79B8A85F-6749-419D-9200-2C6067994E7C}");
            public struct Fields
            {
                public static readonly ID HideInSites = new ID("{67111C0C-0AE5-4C27-B059-94609EB52B0F}");
            } 
        }
    }
}