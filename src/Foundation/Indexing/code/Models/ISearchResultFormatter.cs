namespace Sitecore.Foundation.Indexing.Models
{
    using Sitecore.ContentSearch.SearchTypes;
    using Sitecore.Data;
    using System.Collections.Generic;

    public interface ISearchResultFormatter
    {
        string ContentType { get; }
        IEnumerable<ID> SupportedTemplates { get; }
        void FormatResult(SearchResultItem item, ISearchResult formattedResult);
    }
}