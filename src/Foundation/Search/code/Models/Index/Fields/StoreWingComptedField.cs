namespace Sitecore.Foundation.Search.Models.Index.Fields
{
    using Sitecore.ContentSearch;
    using Sitecore.ContentSearch.ComputedFields;
    using Sitecore.ContentSearch.Utilities;
    using Sitecore.Foundation.FrasersContent;
    using Sitecore.Foundation.SitecoreExtensions.Extensions;
    public class StoreWingComptedField : IComputedIndexField
    {
        public string FieldName { get; set; }
        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            var indexItem = indexable as SitecoreIndexableItem;
            if (indexItem == null || !indexItem.Item.FieldHasValue(Templates.Store.Fields.Wing))
            {
                return null;
            }
            var wingVal = IdHelper.NormalizeGuid(indexItem.Item.Fields[Templates.Store.Fields.Wing].Value);
            return wingVal;
        }
    }
}