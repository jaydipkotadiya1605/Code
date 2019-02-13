using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Globalization;

namespace Sitecore.Foundation.Import.Tasks
{
    public class ImportCommandItem : CustomItem
    {
        public ImportCommandItem(Item item) : base(item)
        {

        }

        public string FileName
        {
            get { return InnerItem[ImportCommandItemName.FileName.ToString()]; }
        }

        public Database ImportDatabase
        {
            get
            {
                var database = Sitecore.Configuration.Factory.GetDatabase(InnerItem[ImportCommandItemName.Database.ToString()]);
                return database;
            }
        }

        public bool FirstRowAsColumnNames
        {
            get { return InnerItem[ImportCommandItemName.FirstRowAsColumnNames.ToString()] == Constants.FirstRowAsColumnNames; }
        }

        public ID ContentTypeId
        {
            get
            {
                ID id;
                if (ID.TryParse(InnerItem[ImportCommandItemName.ContentType.ToString()], out id))
                {
                    return id;
                }
                return null;
            }
        }

        public Language TargetLanguage
        {
            get
            {
                ID id;
                if (!ID.TryParse(InnerItem[ImportCommandItemName.TargetLanguage.ToString()], out id))
                {
                    return null;
                }
                var langItem = Database.GetItem(id);
                if (langItem == null)
                {
                    return null;
                }
                return Language.Parse(langItem.Name);
            }
        }

        public ID ImportMapId
        {
            get
            {
                ID id;
                if (ID.TryParse(InnerItem[ImportCommandItemName.ImportMap.ToString()], out id))
                {
                    return id;
                }
                return null;
            }
        }

        public string CsvDelimiter
        {
            get { return InnerItem[ImportCommandItemName.CsvDelimiter.ToString()]; }
        }

        public string ExistingItemHandling
        {
            get { return GetDropDownItemValue(InnerItem, ImportCommandItemName.ExistingItemHandling.ToString()); }
        }

        public string InvalidLinkHandling
        {
            get { return GetDropDownItemValue(InnerItem, ImportCommandItemName.InvalidLinkHandling.ToString()); }
        }

        private static string GetDropDownItemValue(Item item, string fieldName)
        {
            ID id;
            if (ID.TryParse(item[fieldName], out id))
            {
                var dropDownItem = item.Database.GetItem(id);
                if (dropDownItem != null)
                {
                    return dropDownItem["Name"];
                }
            }
            return null;
        }

        public string MultipleValuesImportSeparator
        {
            get { return InnerItem[ImportCommandItemName.MultipleValuesImportSeparator.ToString()]; }
        }

        public string TreePathValuesImportSeparator
        {
            get { return InnerItem[ImportCommandItemName.TreePathValuesImportSeparator.ToString()]; }
        }
    }
}