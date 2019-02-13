using Sitecore.Data;

namespace Sitecore.Foundation.Import.Map
{
    public class MediaImportMap
    {
        
        public ID TemplateId { get; set; }
        public string InputFileNameFormat { get; set; }
        public string[] MappingFields { get; set; }
        public string ItemIdProperty { get; set; }
        public string ImageFieldProperty { get; set; }
        public bool UseFileNameForMediaItem { get; set; }
        public string MediaItemNameFormat { get; set; }
        public string[] MediaNameMappingFields { get; set; }
        public string AltTextFormat { get; set; }
        public string[] AltTextMappingFields { get; set; }
        public bool IsValid { get; set; }

        private static string[] fileNameFormatDelimiter = new[] { FileNameWordDelimiter };
        public static string[] FileNameFormatDelimiter { get => fileNameFormatDelimiter; set => fileNameFormatDelimiter = value; }

        private static string[] altTextFormatDelimiter = new[] { AltTextWordDelimiter };
        public static string[] AltTextFormatDelimiter { get => altTextFormatDelimiter; set => altTextFormatDelimiter = value; }

        public const string FileNameWordDelimiter = "_";
        public const string AltTextWordDelimiter = " ";
        
    }
}