﻿namespace Sitecore.Foundation.Search.Models.Index.Fields
{
    using System;
    using Sitecore.ContentSearch;
    using Sitecore.ContentSearch.ComputedFields;
    using Sitecore.Foundation.FrasersContent;
    using Sitecore.Foundation.SitecoreExtensions.Extensions;

    public class EndDateComputedField : IComputedIndexField
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

            if (indexItem.Item.FieldHasValue(Templates.Event.Fields.EndDate))
            {
                return DateUtil.IsoDateToDateTime(indexItem.Item.Fields[Templates.Event.Fields.EndDate].Value);
            }

            return null;
        }
    }
}