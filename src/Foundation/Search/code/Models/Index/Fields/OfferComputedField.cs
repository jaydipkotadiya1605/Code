using System.Linq;

namespace Sitecore.Foundation.Search.Models.Index.Fields
{
    using Sitecore.ContentSearch;
    using Sitecore.ContentSearch.ComputedFields;
    using Sitecore.Foundation.SitecoreExtensions.Extensions;
    using Sitecore.Foundation.FrasersContent;

    public class OfferComputedField : IComputedIndexField
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
                .GetMultiListValueItems(Templates.Store.Fields.StoreOffers)
                .Select(x => x.GetString(Templates.StoreOffer.Fields.Value))
                .ToList();
            return items;
        }
    }
}