using Sitecore.Data;
using Sitecore.Foundation.Import.Configuration;
using System;
using System.Text;

namespace Sitecore.Foundation.Import.FieldUpdater
{
    public class TreeListFieldUpdater : IFieldUpdater
    {
        public void UpdateField(Sitecore.Data.Fields.Field field, string importValue, IImportOptions importOptions)
        {
            try
            {
                var separator = new[] {importOptions.MultipleValuesImportSeparator};
                var selectionSource = field.Item.Database.SelectSingleItem(field.Source);
                var importValues = importValue != null
                    ? importValue.Split(separator, StringSplitOptions.RemoveEmptyEntries)
                    : new string[] {};
                var idListValue = string.Empty;
                foreach (var value in importValues)
                {
                    var query = ID.IsID(value)
                        ? string.Format(".//*[@@id='{0}']", ID.Parse(value))
                        : string.Format(".{0}", Sitecore.StringUtil.EnsurePrefix('/', value.Replace(importOptions.TreePathValuesImportSeparator, "/")));
                    var item = selectionSource.Axes.SelectSingleItem(query);
                    idListValue = BuildIdListValue(importOptions, value, item);
                }
                if (idListValue.StartsWith("|"))
                {
                    idListValue = idListValue.Substring(1);
                }
                field.Value = idListValue;
            }
            catch
            {
                if (importOptions.InvalidLinkHandling == InvalidLinkHandling.SetBroken)
                {
                    field.Value = importValue;
                }
                else if (importOptions.InvalidLinkHandling == InvalidLinkHandling.SetEmpty)
                {
                    field.Value = string.Empty;
                }
            }
        }

        private static string BuildIdListValue(IImportOptions importOptions, string value, Data.Items.Item item)
        {
            StringBuilder builder = new StringBuilder();
            if (item != null)
            {
                builder.AppendFormat("|{0}", item.ID);
            }
            else
            {
                if (importOptions.InvalidLinkHandling == InvalidLinkHandling.SetBroken)
                {
                    builder.AppendFormat("|{0}", value);
                }
                else if (importOptions.InvalidLinkHandling == InvalidLinkHandling.SetEmpty)
                {
                    builder.Append("|");
                }
            }
            return builder.ToString();
        }
    }
}