namespace Sitecore.Foundation.Search.Models.Index.Fields
{
    using Sitecore.ContentSearch;
    using Sitecore.ContentSearch.ComputedFields;
    using Sitecore.Foundation.FrasersContent;
    using Sitecore.Foundation.SitecoreExtensions.Extensions;

    public class StoreNextThreeMonthsNewDateComputedField : IComputedIndexField
    {
        public string FieldName { get; set; }
        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            var indexItem = indexable as SitecoreIndexableItem;
            if (indexItem == null || !indexItem.Item.FieldHasValue(Templates.Store.Fields.NewDate))
            {
                return null;
            }
            var newDate = indexItem.Item.GetDateTime(Templates.Store.Fields.NewDate);
            return newDate.AddMonths(3);
        }
    }
}