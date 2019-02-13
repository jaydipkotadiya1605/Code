namespace Sitecore.Foundation.Indexing.Models
{
  using System;
  using System.Collections.Generic;
  using System.Linq.Expressions;
  using Sitecore.ContentSearch.SearchTypes;
  using Sitecore.Data;

  public interface IQueryPredicateProvider
  {
    Expression<Func<T, bool>> GetQueryPredicate<T>(IQuery query) where T: SearchResultItem;
    IEnumerable<ID> SupportedTemplates { get; }
  }
}