namespace Sitecore.Foundation.Search.Models.Index.Fields
{
    using Sitecore.ContentSearch;
    using Sitecore.ContentSearch.ComputedFields;
    using Sitecore.ContentSearch.Utilities;
    using Sitecore.Foundation.SitecoreExtensions.Extensions;

    public class StoreMallSiteComputedField : IComputedIndexField
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

            var site = indexItem.Item.GetAncestorOrSelfOfTemplate(Multisite.Templates.Site.ID);
            if (site == null)
            {
                return null;
            }
            return IdHelper.NormalizeGuid(site.ID);
        }
    }
}