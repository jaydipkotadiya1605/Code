using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Foundation.Import.Configuration;
using System;
using System.Linq;
using System.Text;

namespace Sitecore.Foundation.Import.FieldUpdater
{
    public abstract class ListFieldUpdater
    {
        public void UpdateField(Field field, string importValue, IImportOptions importOptions)
        {
            var separator = new[] { importOptions.MultipleValuesImportSeparator };
            var selectionSource = field.Item.Database.SelectSingleItem(field.Source);
            if (selectionSource != null)
            {
                var strValue = GetFieldValue(selectionSource, importValue, separator, importOptions, field);
                field.Value = strValue;
                return;
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

        private string GetFieldValue(Item selectionSource, string importValue, string[] separator, IImportOptions importOptions, Field field)
        {
            var importValues = importValue != null
                    ? importValue.Split(separator, StringSplitOptions.RemoveEmptyEntries)
                    : new string[] { };
            StringBuilder idListValue = new StringBuilder();
            foreach (var value in importValues)
            {
                var isIdImportValue = ID.IsID(value);
                var selectedItem = isIdImportValue
                    ? selectionSource.Children[ID.Parse(value)]
                    : selectionSource.Children[value];
                if (selectedItem != null)
                {
                    idListValue.Append("|" + selectedItem.ID);
                }
                else
                {
                    UpdateFieldValueByInvalidLinkHandling(importOptions, idListValue, selectionSource, field, value);
                }
            }
            var idListValueStr = idListValue.ToString();
            if (idListValueStr.StartsWith("|"))
            {
                idListValueStr = idListValueStr.Substring(1);
            }

            return idListValueStr;
        }

        private void UpdateFieldValueByInvalidLinkHandling(IImportOptions importOptions, StringBuilder idListValue, Item selectionSource, Field field, string value)
        {
            if (importOptions.InvalidLinkHandling == InvalidLinkHandling.CreateItem)
            {
                var firstChild = selectionSource.Children.FirstOrDefault();
                if (firstChild != null)
                {
                    var template = field.Item.Database.GetTemplate(firstChild.TemplateID);
                    var itemName = Utils.GetValidItemName(value);
                    var createdItem = selectionSource.Add(itemName, template);
                    if (createdItem != null)
                    {
                        idListValue.Append("|" + createdItem.ID.ToString());
                    }
                }
            }
            else if (importOptions.InvalidLinkHandling == InvalidLinkHandling.SetBroken)
            {
                idListValue.Append("|" + value);
            }
        }
    }
}