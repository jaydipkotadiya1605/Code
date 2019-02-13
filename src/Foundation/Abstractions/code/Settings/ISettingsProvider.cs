namespace Sitecore.Foundation.Abstractions.Settings
{
    public interface ISettingsProvider
    {
        string GetSetting(string name);
        string GetSetting(string name, string defaultValue);
    }
}
