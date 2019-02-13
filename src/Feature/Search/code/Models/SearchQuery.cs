using System.Collections.Generic;

namespace Sitecore.Feature.Search.Models
{
    using Sitecore.Foundation.Indexing.Models;

    public class SearchQuery : IQuery
    {
        public string QueryText { get; set; }
        public int NoOfResults { get; set; }
        public Dictionary<string, string[]> Facets { get; set; }
        public int Page { get; set; }
    }
}