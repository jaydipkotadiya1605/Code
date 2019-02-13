namespace Sitecore.Foundation.Import.Models
{
    public class ImportModel
    {
        public string ContentTypeId { get; set; }
        public string Language { get; set; }
        public string ExistingItemHandling { get; set; }
        public string InvalidLinkHandling { get; set; }
        public string CsvDelimiter { get; set; }
        public string MultipleValuesSeparator { get; set; }
        public string MediaItemId { get; set; }
        public bool FirstRowAsColumnNames { get; set; }
    }
}