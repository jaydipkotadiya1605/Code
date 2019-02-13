namespace Sitecore.Foundation.Abstractions.Settings
{
    using Sitecore.Configuration;
    using Sitecore.Foundation.DependencyInjection;

    [Service(typeof(ISettingsProvider))]
    public class SettingsProvider : ISettingsProvider
    {
        public string GetSetting(string name)
        {
            return Settings.GetSetting(name);
        }

        public string GetSetting(string name, string defaultValue)
        {
            return Settings.GetSetting(name, defaultValue);
        }
    }
}