using Sitecore.Foundation.Import.Configuration;
using Sitecore.Data.Fields;
using System;

namespace Sitecore.Foundation.Import.FieldUpdater
{
    public static class FieldUpdateManager
    {
        public static void UpdateField(Field field, string importValue, IImportOptions importOptions)
        {
            IFieldUpdater updater = GetFieldUpdater(field);
            updater.UpdateField(field, importValue, importOptions);
        }

        private static IFieldUpdater GetFieldUpdater(Field field)
        {
            if (field.Type.Equals(FieldType.Droplink.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return new DropLinkFieldUpdater();
            }
            if (field.Type.Equals(FieldType.Droptree.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return new DropTreeFieldUpdater();
            }
            if (field.Type.Equals(FieldType.Checklist.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return new CheckBoxListFieldUpdater();
            }
            if (field.Type.Equals(FieldType.Treelist.ToString(), StringComparison.OrdinalIgnoreCase) ||
                field.Type.Equals(FieldType.TreelistEx.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return new TreeListFieldUpdater();
            }
            if (field.Type.Equals("Datetime", StringComparison.OrdinalIgnoreCase))
            {
                return new DatetimeUpdater();
            }
            return new TextFieldUpdater();
        }
    }
}