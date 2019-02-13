namespace Sitecore.Foundation.Indexing.Models
{
    using Sitecore.Data.Items;

    public interface ISearchResult
    {
        Item Item { get; }
        string Id { get; }
        string Title { get; set; }
        string ContentType { get; set; }
        string Description { get; set; }
        string Category { get; set; }
        string CategoryUrl { get; set; }
        string Url { get; set; }
        string ViewName { get; set; }
        MediaItem Media { get; set; }
    }
}