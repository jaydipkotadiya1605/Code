namespace Sitecore.Foundation.Search.Models.Index.Fields
{
    using Sitecore.ContentSearch;
    using Sitecore.ContentSearch.ComputedFields;
    using Sitecore.Foundation.FrasersContent;
    using Sitecore.Foundation.SitecoreExtensions.Extensions;

    public class ExpiryDateHasValueComputedField : IComputedIndexField
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

            var item = indexItem.Item
                .FieldHasValue(Templates.SchedulableSetting.Fields.ExpiryDate);

            return item;
        }
    }
}