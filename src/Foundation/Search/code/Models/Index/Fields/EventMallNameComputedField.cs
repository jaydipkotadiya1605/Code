using System.Linq;

namespace Sitecore.Foundation.Search.Models.Index.Fields
{
    using Sitecore.ContentSearch;
    using Sitecore.ContentSearch.ComputedFields;
    using Sitecore.Foundation.Dictionary.Repositories;
    using Sitecore.Foundation.FrasersContent;
    using Sitecore.Foundation.SitecoreExtensions.Extensions;

    public class EventMallNameComputedField : IComputedIndexField
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

            string mallName = null;

            var items = indexItem.Item
                .GetMultiListValueItems(Templates.MallSite.Fields.DisplayOnMalls)
                .Select(x => x.ID).ToList();
            if (items.Count == 1)
            {
                mallName = indexItem.Item.Database.GetItem(items[0]).GetString(Templates.Identity.Fields.SiteName);
            }
            else if (items.Count > 1)
            {
                mallName = DictionaryPhraseRepository.Current.Get(Constants.MultipleMalls, Constants.MultipleMallsText);
            }
                
            return mallName;
        }
    }
}