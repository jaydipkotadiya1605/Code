using System.Linq;

namespace Sitecore.Foundation.Search.Models.Index.Fields
{
    using System;
    using Sitecore.ContentSearch;
    using Sitecore.ContentSearch.ComputedFields;
    using Sitecore.Foundation.FrasersContent;
    using Sitecore.Foundation.SitecoreExtensions.Extensions;

    public class StoreExpiryDateComputedField : IComputedIndexField
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

            var expiryDate = DateTime.MaxValue;

            if (indexItem.Item.FieldHasValue(Templates.SchedulableContent.Fields.ExpiryDate))
            {
                expiryDate = DateUtil.IsoDateToDateTime(indexItem.Item.Fields[Templates.SchedulableContent.Fields.ExpiryDate].Value);
            }

            return expiryDate;
        }
    }
}