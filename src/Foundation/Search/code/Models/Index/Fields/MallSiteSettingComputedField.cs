using System.Linq;

namespace Sitecore.Foundation.Search.Models.Index.Fields
{
    using Sitecore.ContentSearch;
    using Sitecore.ContentSearch.ComputedFields;
    using Sitecore.Foundation.SitecoreExtensions.Extensions;

    public class MallSiteSettingComputedField : IComputedIndexField
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
                .GetDroplinkItem(Foundation.Multisite.Templates.MallSiteSetting.Fields.MainSite);
            if (item == null || item.IsDerived(Multisite.Templates.MainSite.ID))
            {
                return null;
            }

            return item.DisplayName;
        }
    }
}