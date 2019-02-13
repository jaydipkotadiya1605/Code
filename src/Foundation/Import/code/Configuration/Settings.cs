namespace Sitecore.Foundation.Import.Configuration
{
    public class Settings
    {
        public static Settings GetConfigurationSettings()
        {
            return new Settings
            {
                MapsLocation = Sitecore.Configuration.Settings.GetSetting("Sitecore.Foundation.Import.MapsLocation", ""),
                RootItemQuery = Sitecore.Configuration.Settings.GetSetting("Sitecore.Foundation.Import.RootItemQuery", ""),
                ImportDirectory =
                    Sitecore.Configuration.Settings.GetSetting("Sitecore.Foundation.Import.ImportDirectory", "~/temp/Importer"),
                ImportItemsSubDirectory =
                    Sitecore.Configuration.Settings.GetSetting("Sitecore.Foundation.Import.ImportItemsSubDirectory", "Items"),
                ImportMediaSubDirectory =
                    Sitecore.Configuration.Settings.GetSetting("Sitecore.Foundation.Import.ImportMediaSubDirectory", "Items")
            };
        }

        public string MapsLocation { get; set; }
        public string RootItemQuery { get; set; }
        public string ImportDirectory { get; set; }
        public string ImportItemsSubDirectory { get; set; }
        public string ImportMediaSubDirectory { get; set; }
    }
}