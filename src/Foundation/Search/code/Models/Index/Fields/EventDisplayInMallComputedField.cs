using System.Linq;

namespace Sitecore.Foundation.Search.Models.Index.Fields
{
    using Sitecore.ContentSearch;
    using Sitecore.ContentSearch.ComputedFields;
    using Sitecore.Foundation.SitecoreExtensions.Extensions;
    using Sitecore.Foundation.FrasersContent;
    using Sitecore.ContentSearch.Utilities;

    public class EventDisplayInMallComputedField : IComputedIndexField
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

            var items = indexItem.Item
                .GetMultiListValueItems(Templates.MallSite.Fields.DisplayOnMalls)
                .Select(x => IdHelper.NormalizeGuid(x.ID))
                .ToList();
            return items;
        }
    }
}