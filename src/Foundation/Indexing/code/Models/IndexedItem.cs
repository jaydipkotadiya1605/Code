﻿namespace Sitecore.Foundation.Indexing.Models
{
    using Sitecore.ContentSearch;
    using Sitecore.ContentSearch.SearchTypes;
    using System.Collections.Generic;

    public class IndexedItem : SearchResultItem
  {
    [IndexField(Constants.IndexFields.HasPresentation)]
    public bool HasPresentation { get; set; }

    [IndexField(Templates.IndexedItem.Fields.IncludeInSearchResults_FieldName)]
    public bool ShowInSearchResults { get; set; }

    [IndexField(Constants.IndexFields.AllTemplates)]
    public List<string> AllTemplates { get; set; }

    [IndexField(Constants.IndexFields.HasSearchResultFormatter)]
    public bool HasSearchResultFormatter { get; set; }

    [IndexField(Constants.IndexFields.IsLatestVersion)]
    public bool IsLatestVersion { get; set; }
  }
}