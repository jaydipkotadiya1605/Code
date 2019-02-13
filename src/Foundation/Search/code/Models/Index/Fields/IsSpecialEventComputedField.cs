﻿namespace Sitecore.Foundation.Search.Models.Index.Fields
{
    using Sitecore.ContentSearch;
    using Sitecore.ContentSearch.ComputedFields;
    using Sitecore.Foundation.FrasersContent;
    using Sitecore.Foundation.SitecoreExtensions.Extensions;

    public class IsSpecialEventComputedField : IComputedIndexField
    {
        public string FieldName { get; set; }
        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            var indexItem = indexable as SitecoreIndexableItem;
            if (indexItem == null)
            {
                return null;
            }

            var isSpecialEvent = indexItem.Item
                .IsDerived(Templates.SpecialEvent.ID);

            return isSpecialEvent;
        }
    }
}