namespace Sitecore.Feature.Search.ViewModels
{
    using System.Web;

    public class SearchHeaderViewModel
    {
        public HtmlString Title { get; set; }
        public HtmlString Subtitle { get; set; }
        public string PlaceholderText { get; set; }
        public HtmlString ButtonSearchText { get; set; }
        public string SearchResultPage { get; set; }
        public string SearchResultPageUrl { get; set; }
    }
}