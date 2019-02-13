namespace Sitecore.Foundation.MediaInfo
{
    public struct Constants
    {
        public const string TemplateFolder = "System/Media";
        public const string CollapsedSetting = "/Current_User/Content Editor/Sections/Collapsed";

        public const string SectionName = "MediaInfo";
        public const string ControlId = "MediaInfo";
        public const string DisplayName = "Media Quick Info";
        public const string Icon = "People/32x32/atom.png";

        public static readonly string True = "1";

        public struct MediaInfo
        {
            public const string UrlById = "Url by ID";
            public const string UrlByPath = "Url by Path";
            public const string MediaLocation = "Location";
        }

        public struct MediaType
        {
            public const string FileSystem = "File System";
            public const string Database = "Database";
        }

        public struct DisplayFormat
        {
            public const string OpenTable = "<table class=\"scEditorQuickInfo\"><colgroup><col style =\"white-space:nowrap\" valign=\"top\" /><col style=\"white-space:nowrap\" valign=\"top\" /><colgroup><tbody>";
            public const string CloseTable = "</tbody></table>";
            public const string Row = "<tr><td>{0}</td><td><input class=\"scEditorHeaderQuickInfoInput\" readonly=\"readonly\" onclick=\"javascript:this.select();return false\" value=\"{1}\"></td></tr>";
        }
    }
}