namespace Sitecore.Foundation.Indexing.Models
{
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using System.Collections.Generic;

    public class SearchSettingsBase : ISearchSettings
    {
        public Item Root { get; set; }
        public IEnumerable<ID> Templates { get; set; }
        public bool MustHaveFormatter { get; set; } = false;
    }
}