namespace Sitecore.Feature.Blog.Indexing
{
    using Sitecore.ContentSearch.SearchTypes;
    using Sitecore.Data;
    using Sitecore.Data.Fields;
    using Sitecore.Foundation.Dictionary.Repositories;
    using Sitecore.Foundation.FrasersContent;
    using Sitecore.Foundation.Indexing.Infrastructure;
    using Sitecore.Foundation.Indexing.Models;
    using Sitecore.Foundation.SitecoreExtensions.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Configuration.Provider;
    using System.Linq;
    using System.Linq.Expressions;
    using Sitecore.Foundation.ItemResolver.Extensions;

    public class BlogIndexingProvider : ProviderBase, ISearchResultFormatter, IQueryPredicateProvider
    {
        public IEnumerable<ID> SupportedTemplates => new[] { Templates.Blog.ID };

        public string ContentType => DictionaryPhraseRepository.Current.Get(Blog.Constants.ContentType, Blog.Constants.ContentTypeName);

        public Expression<Func<T, bool>> GetQueryPredicate<T>(IQuery query) where T: SearchResultItem
        {
            var fieldNames = new[]
            {
                Templates.Blog.Fields.Title_FieldName,
                Templates.Blog.Fields.Summary_FieldName,
                Templates.Blog.Fields.Body_FieldName,
                Templates.Blog.Fields.Author_FieldName,
            };
            return GetFreeTextPredicateService.GetFreeTextPredicate<T>(fieldNames, query);
        }

        public void FormatResult(SearchResultItem item, ISearchResult formattedResult)
        {
            var contentItem = item.GetItem();
            if (contentItem == null)
            {
                return;
            }
            formattedResult.Title = contentItem.GetString(Templates.Blog.Fields.Title);
            formattedResult.Description = contentItem.GetString(Templates.Blog.Fields.Summary);
            formattedResult.Media = ((ImageField)contentItem.Fields[Templates.Blog.Fields.Banner])?.MediaItem;
            var categories = contentItem.GetMultiListValueItems(Templates.Blog.Fields.Category);
            var categoryNames = categories?.Select(category => category.GetString(Templates.Blog.BlogCategory.Fields.Value)).ToList();
            formattedResult.Category = string.Join(", ", categoryNames ?? Enumerable.Empty<string>());
            formattedResult.CategoryUrl = contentItem.GetParent()?.Url();
        }
    }
}