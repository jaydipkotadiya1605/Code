using System;

namespace Sitecore.Foundation.Import.Configuration
{
    public static class Factory
    {
        public static IImportOptions GetDefaultImportOptions()
        {
            var value = Sitecore.Configuration.Settings.GetSetting("Sitecore.Foundation.Import.ExistingItemHandling", "AddVersion");
            ExistingItemHandling existingItemHandling;
            if (!Enum.TryParse<ExistingItemHandling>(value, out existingItemHandling))
            {
                existingItemHandling = Sitecore.Foundation.Import.ExistingItemHandling.AddVersion;
            }

            var invalidLinkHandlingValue = Sitecore.Configuration.Settings.GetSetting("Sitecore.Foundation.Import.InvalidLinkHandling",
                "SetBroken");
            InvalidLinkHandling invalidLinkHandling;
            if (!Enum.TryParse<InvalidLinkHandling>(invalidLinkHandlingValue, out invalidLinkHandling))
            {
                invalidLinkHandling = Sitecore.Foundation.Import.InvalidLinkHandling.SetBroken;
            }

            return new ImportOptions
            {
                ExistingItemHandling = existingItemHandling,
                InvalidLinkHandling = invalidLinkHandling,
                MultipleValuesImportSeparator =
                    Sitecore.Configuration.Settings.GetSetting("Sitecore.Foundation.Import.MultipleValuesImportSeparator", "|"),
                TreePathValuesImportSeparator =
                    Sitecore.Configuration.Settings.GetSetting("Sitecore.Foundation.Import.TreePathValuesImportSeparator", @"\"),
                CsvDelimiter = new[]
                {
                    Sitecore.Configuration.Settings.GetSetting("Sitecore.Foundation.Import.CsvDelimiter", ",")
                },
                FirstRowAsColumnNames = Sitecore.Configuration.Settings.GetBoolSetting("Sitecore.Foundation.Import.FirstRowAsColumnNames", true)
            };
        }
    }
}