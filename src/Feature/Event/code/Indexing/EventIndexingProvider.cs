namespace Sitecore.Feature.Event.Indexing
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

    public class EventIndexingProvider : ProviderBase, ISearchResultFormatter, IQueryPredicateProvider
    {
        public IEnumerable<ID> SupportedTemplates => new[] { Templates.Event.ID };

        public string ContentType => DictionaryPhraseRepository.Current.Get(Event.Constants.ContentType, Event.Constants.ContentTypeName);

        public Expression<Func<T, bool>> GetQueryPredicate<T>(IQuery query) where T: SearchResultItem
        {
            var fieldNames = new[]
            {
                Templates.Event.Fields.Title_FieldName,
                Templates.Event.Fields.Summary_FieldName,
                Templates.Event.Fields.Description_FieldName
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
            formattedResult.Title = contentItem.GetString(Templates.Event.Fields.Title);
            formattedResult.Description = contentItem.GetString(Templates.Event.Fields.Summary);
            formattedResult.Media = ((ImageField)contentItem.Fields[Templates.Event.Fields.Image])?.MediaItem;
            var category = contentItem.GetDroplinkItem(Templates.Event.Fields.Category);
            if (category != null)
            {
                formattedResult.Category = category.GetString(Templates.EventCategory.Fields.Value);
                formattedResult.CategoryUrl = contentItem.GetParent()?.Url();
            }
        }
    }
}