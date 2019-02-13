namespace Sitecore.Foundation.Search.Models
{
    public class PageSettingExtend
    {
        public int TotalNumberOfResults { get; set; }
        public bool HasMoreResult { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}