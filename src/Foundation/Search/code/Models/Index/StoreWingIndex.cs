namespace Sitecore.Foundation.Search.Models.Index
{
    using Sitecore.ContentSearch;
    using Sitecore.Foundation.Indexing.Models;

    public class StoreWingIndex : IndexedItem
    {
        [IndexField("key_s")]
        public string Key { get; set; }

        [IndexField("value_s")]
        public string Value { get; set; }
    }
}