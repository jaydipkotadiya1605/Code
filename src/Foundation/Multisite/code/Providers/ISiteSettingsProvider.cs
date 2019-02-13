namespace Sitecore.Foundation.Multisite.Providers
{
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Sitecore.Links;
    using Sitecore.Resources.Media;

    public interface ISiteSettingsProvider
    {
        Item GetSetting(Item contextItem, string settingsType, string setting);

        Item GetSetting(Item contextItem, string settingName);

        Item GetSetting(Item contextItem, ID baseSettingId);

        UrlOptions GetUrlOptions();

        MediaUrlOptions GetMediaUrlOptions();
    }
}