using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Foundation.Import.Configuration;
using System.Linq;

namespace Sitecore.Foundation.Import.FieldUpdater
{
    public class DropLinkFieldUpdater : IFieldUpdater
    {
        public void UpdateField(Field field, string importValue, IImportOptions importOptions)
        {
            var dataSource = field.Source;
            Item[] queryItems = null;
            Item selectionSource = null;
            var isQuery = IsQuery(dataSource);
            var isDatasourceId = IsDatasourceId(dataSource);
            if (isQuery)
            {
                string query = dataSource.Substring(Constants.DatasourceStartWithQuery.Length);
                queryItems = field.Item.Parent.Database.SelectItems(query);
            }
            else
                selectionSource = GetSelectionSource(isDatasourceId, dataSource, field);

            var isIdImportValue = ID.IsID(importValue);
            Item selectedItem = null;
            if (selectionSource != null || queryItems.Any())
            {

                selectedItem = GetSelectedItem(isQuery, isIdImportValue, queryItems, importValue, selectionSource);
                if (selectedItem != null)
                {
                    field.Value = selectedItem.ID.ToString();
                    return;
                }
                SetForInvalidLinkHandling(field, importOptions, selectionSource, isIdImportValue, isQuery, importValue);
            }
            if (importOptions.InvalidLinkHandling == InvalidLinkHandling.SetBroken)
            {
                field.Value = importValue;
            }
            else if (importOptions.InvalidLinkHandling == InvalidLinkHandling.SetEmpty)
            {
                field.Value = string.Empty;
            }
        }

        private Item GetSelectionSource(bool isDatasourceId, string dataSource, Field field)
        {
            Item selectionSource;
            if (isDatasourceId)
            {
                var id = dataSource.Substring(Constants.DatasourceStartWithDatasource.Length);
                selectionSource = field.Item.Database.GetItem(new ID(id));
            }
            else
                selectionSource = field.Item.Database.SelectSingleItem(dataSource);

            return selectionSource;
        }

        private Item GetSelectedItem(bool isQuery, bool isIdImportValue, Item[] queryItems, string importValue, Item selectionSource)
        {
            Item selectedItem = null;
            if (isQuery)
                selectedItem = isIdImportValue
                ? queryItems.FirstOrDefault(x => x.ID.Equals(ID.Parse(importValue)))
                : queryItems.FirstOrDefault(x => x.ID.Equals(importValue));
            else if (selectionSource != null)
                selectedItem = isIdImportValue
                ? selectionSource.Children[ID.Parse(importValue)]
                : selectionSource.Children[importValue];

            return selectedItem;
        }
        private bool IsQuery(string dataSource)
        {
            if (dataSource.StartsWith(Constants.DatasourceStartWithQuery))
                return true;
            return false;
        }
        private bool IsDatasourceId(string dataSource)
        {
            if (dataSource.StartsWith(Constants.DatasourceStartWithDatasource))
                return true;
            return false;
        }

        private void SetForInvalidLinkHandling(Field field, IImportOptions importOptions, Item selectionSource, bool isIdImportValue, bool isQuery, string importValue)
        {
            if (importOptions.InvalidLinkHandling == InvalidLinkHandling.CreateItem && !isIdImportValue && !isQuery)
            {
                var firstChild = selectionSource.Children.FirstOrDefault();
                if (firstChild != null)
                {
                    var template = field.Item.Database.GetTemplate(firstChild.TemplateID);
                    var itemName = Utils.GetValidItemName(importValue);
                    var createdItem = selectionSource.Add(itemName, template);
                    if (createdItem != null)
                    {
                        field.Value = createdItem.ID.ToString();
                    }
                }
            }
        }
    }
}