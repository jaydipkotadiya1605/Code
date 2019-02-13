namespace Sitecore.Foundation.Indexing.Models
{
    using Sitecore.ContentSearch.Utilities;
    using Sitecore.Data.Items;
    using Sitecore.Foundation.SitecoreExtensions.Extensions;

    public class SearchResult : ISearchResult
    {
        private string _url;

        public SearchResult(Item item)
        {
            this.Item = item;
        }

        public Item Item { get; }
        public MediaItem Media { get; set; }
        public string Title { get; set; }
        public string ContentType { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string CategoryUrl { get; set; }
        public string Id => IdHelper.NormalizeGuid(this.Item.ID);
        public string Url
        {
            get
            {
                return this._url ?? this.Item?.Url();
            }
            set
            {
                this._url = value;
            }
        }

        public string ViewName { get; set; }
    }
}