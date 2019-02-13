using System.Linq;

namespace Sitecore.Foundation.Search.Models.Index.Fields
{
    using System;
    using Sitecore.ContentSearch;
    using Sitecore.ContentSearch.ComputedFields;
    using Sitecore.Foundation.FrasersContent;
    using Sitecore.Foundation.SitecoreExtensions.Extensions;

    public class PostDateComputedField : IComputedIndexField
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

            var expiryDate = DateTime.MinValue;

            if (indexItem.Item.FieldHasValue(Templates.SchedulableContent.Fields.PostDate))
            {
                expiryDate = DateUtil.IsoDateToDateTime(indexItem.Item.Fields[Templates.SchedulableContent.Fields.PostDate].Value);
            }

            return expiryDate;
        }
    }
}