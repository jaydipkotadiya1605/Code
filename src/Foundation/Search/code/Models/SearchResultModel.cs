namespace Sitecore.Foundation.Search.Models
{
    using Sitecore.Data.Items;

    public class SearchResultModel<T>
    {
        public Item Item { get; set; }
        public T Document { get; set; }
    }
}