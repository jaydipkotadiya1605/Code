namespace Sitecore.Feature.Identity
{
    using Sitecore.Data;

    public struct Templates
    {
        public struct Identity
        {
            public static readonly ID ID = new ID("{FA8DE5B9-D5D8-40A7-866A-23AF4F5A9629}");

            public struct Fields
            {
                public static readonly ID Logo = new ID("{E748D808-64C1-4DEC-9718-F35CF9689E4B}");
                public static readonly ID Favicon = new ID("{D7FAF785-FF32-4F18-8D43-814651C11F00}");
                public static readonly ID SiteName = new ID("{D390B56F-6F6C-4DA7-832C-2ED4C44733E5}");
            }
        }

        public struct FooterLink
        {
            public static readonly ID ID = new ID("{DBE85A63-54AF-4FFF-ABFC-C4C1652BD3AD}");

            public struct Fields
            {
                public static readonly ID Link = new ID("{1FE0B19A-A101-4F70-B8A7-81A032E626F0}");
                public static readonly ID ShowOnMobile = new ID("{8CAAC899-17F3-4EF1-83EA-A4BD6C372C93}");
            }
        }

        public struct Footer
        {
            public static readonly ID ID = new ID("{468FF711-C45D-4145-B9AD-E0B71705217C}");

            public struct Fields
            {
                public static readonly ID Logo = new ID("{C039AF0A-D6FD-4E75-A60E-01689633C9B9}");
                public static readonly ID Menu = new ID("{875B1529-A3B1-4221-8DD6-F6DF37521B1B}");
                public static readonly ID IncludeHeaderSocialIcons = new ID("{2470E69E-3054-495D-887A-4EB174B04BC4}");
            }

            public struct Copyright
            {
                public struct Fields
                {
                    public static readonly ID Logo = new ID("{2DA4F169-E77A-42CE-8B85-BAE3515C0323}");
                    public static readonly ID Text = new ID("{B5958343-38CD-4A08-9B72-EB8FD145B463}");
                    public static readonly ID Links = new ID("{59C2D766-5657-429B-902E-3B6E406EB41B}");
                }
            }
        }

        public struct HeaderSettings
        {
            public static readonly ID ID = new ID("{A1E45B2D-4CF4-4F4A-B922-6EBB7175DD64}");

            public struct MainLinks
            {
                public static readonly ID ID = new ID("{C6A18D6F-DFE2-43DB-B7FE-E4BD68C4C77E}");
                public struct Fields
                {
                    public static readonly ID Link = new ID("{43946489-9BBE-4B7C-9257-F187BDFD4B7A}");
                }
            }

            public struct SocialLinks
            {
                public static readonly ID ID = new ID("{C0666526-56C5-42FA-A8CF-F70FBFDA0508}");
                public struct Fields
                {
                    public static readonly ID Icon = new ID("{AB91D564-2B6A-48F1-AAC0-DE65EA773DE2}");
                    public static readonly ID MobileIcon = new ID("{8FCFF3A4-3D0C-4060-895F-EC7CE17317A6}");
                    public static readonly ID Css = new ID("{73122077-C90E-4FC3-9954-0E2287F934F9}");
                    public static readonly ID Link = new ID("{6B8119E3-0258-47AA-BAF7-6F3093F53167}");
                    public static readonly ID IconPostText = new ID("{295992FD-D41D-459A-ADA4-18FD482CB0C8}");
                }
            }

            public struct Settings {

                public static readonly ID ID = new ID("{4C82B6DD-FE7C-4144-BCB3-F21B4080568F}");
            }
        }
    }
}