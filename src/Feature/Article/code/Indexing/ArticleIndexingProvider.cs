﻿namespace Sitecore.Feature.Article.Indexing
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
    using System.Linq.Expressions;
    using Sitecore.Foundation.ItemResolver.Extensions;

    public class ArticleIndexingProvider : ProviderBase, ISearchResultFormatter, IQueryPredicateProvider
    {
        public IEnumerable<ID> SupportedTemplates => new[] { Templates.Article.ID };

        public string ContentType => DictionaryPhraseRepository.Current.Get(Article.Constants.ContentType, Article.Constants.ContentTypeName);

        public Expression<Func<T, bool>> GetQueryPredicate<T>(IQuery query) where T: SearchResultItem
        {
            var fieldNames = new[]
            {
                Templates.Article.Fields.Title_FieldName,
                Templates.Article.Fields.Summary_FieldName,
                Templates.Article.Fields.Description_FieldName,
                Templates.Article.Fields.BookingId_FieldName
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
            formattedResult.Title = contentItem.GetString(Templates.Article.Fields.Title);
            formattedResult.Description = contentItem.GetString(Templates.Article.Fields.Summary);
            formattedResult.Media = ((ImageField)contentItem.Fields[Templates.Article.Fields.Banner])?.MediaItem;
            var category = contentItem.GetDroplinkItem(Templates.Article.Fields.Category);
            if (category != null)
            {
                formattedResult.Category = category.GetString(Templates.ArticleCategory.Fields.Value);
                formattedResult.CategoryUrl = contentItem.GetParent()?.Url();
            }
        }
    }
}