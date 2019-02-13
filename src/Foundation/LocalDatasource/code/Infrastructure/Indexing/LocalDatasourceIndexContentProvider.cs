namespace Sitecore.Foundation.LocalDatasource.Infrastructure.Indexing
{
    using Sitecore.ContentSearch.SearchTypes;
    using Sitecore.Data;
    using Sitecore.Foundation.Indexing.Infrastructure;
    using Sitecore.Foundation.Indexing.Models;
    using System;
    using System.Collections.Generic;
    using System.Configuration.Provider;
    using System.Linq.Expressions;

    public class LocalDatasourceQueryPredicateProvider : ProviderBase, IQueryPredicateProvider
    {
        public IEnumerable<ID> SupportedTemplates => new[]
        {
          TemplateIDs.StandardTemplate
        };

        public Expression<Func<T, bool>> GetQueryPredicate<T>(IQuery query) where T: SearchResultItem
        {
            var fieldNames = new[]
            {
        Templates.Index.Fields.LocalDatasourceContent_IndexFieldName
      };
            return GetFreeTextPredicateService.GetFreeTextPredicate<T>(fieldNames, query);
        }
    }
}