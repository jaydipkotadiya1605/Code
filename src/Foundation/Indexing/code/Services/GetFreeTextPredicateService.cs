namespace Sitecore.Foundation.Indexing.Infrastructure
{
    using Sitecore.ContentSearch.Linq.Utilities;
    using Sitecore.ContentSearch.SearchTypes;
    using Sitecore.Foundation.Indexing.Models;
    using System;
    using System.Linq.Expressions;

    public static class GetFreeTextPredicateService
    {
        public static Expression<Func<T, bool>> GetFreeTextPredicate<T>(string[] fieldNames, IQuery query) where T: SearchResultItem
        {
            var predicate = PredicateBuilder.False<T>();
            if (string.IsNullOrWhiteSpace(query.QueryText))
            {
                return predicate;
            }
            foreach (var name in fieldNames)
            {
                predicate = predicate.Or(i => i[name].Contains(query.QueryText));
            }
            return predicate;
        }
    }
}