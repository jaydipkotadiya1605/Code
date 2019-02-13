namespace Sitecore.Foundation.Search.Models.Index.Fields
{
    using Sitecore.ContentSearch;
    using Sitecore.ContentSearch.ComputedFields;
    using Sitecore.ContentSearch.Utilities;
    using Sitecore.Foundation.FrasersContent;
    using Sitecore.Foundation.SitecoreExtensions.Extensions;
    using System.Linq;
    using System.Text;

    public class BlogCategoryComputedField : IComputedIndexField
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
                .GetMultiListValueItems(Templates.Blog.Fields.Category);
            if (items == null || !items.Any())
            {
                return null;
            }
            StringBuilder category = new StringBuilder();
            foreach (var item in items)
            {
                if (string.IsNullOrEmpty(category.ToString()))
                {
                    category.Append(IdHelper.NormalizeGuid(item.ID));
                }
                else
                {
                    category.Append(",");
                    category.Append(IdHelper.NormalizeGuid(item.ID));
                }
            }

            return category;
        }
    }
}