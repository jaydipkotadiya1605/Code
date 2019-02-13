namespace Sitecore.Foundation.Multisite.Providers
{
    using Sitecore.Configuration;
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Sitecore.Foundation.DependencyInjection;
    using Sitecore.Foundation.SitecoreExtensions.Extensions;
    using Sitecore.Links;
    using Sitecore.Resources.Media;
    using System.Linq;

    [Service(typeof(ISiteSettingsProvider))]
    public class SiteSettingsProvider : ISiteSettingsProvider
    {
        private readonly SiteContext siteContext;

        public SiteSettingsProvider() : this(new SiteContext())
        {
        }

        public SiteSettingsProvider(SiteContext siteContext)
        {
            this.siteContext = siteContext;
        }

        public static string SettingsRootName => Settings.GetSetting("Multisite.SettingsRootName", "settings");

        public virtual Item GetSetting(Item contextItem, string settingsType, string setting)
        {
            var settingsRootItem = this.GetSettingsRoot(contextItem, settingsType);
            var settingItem = settingsRootItem?.Children.FirstOrDefault(i => i.Key.Equals(setting.ToLower()));
            return settingItem;
        }

        public virtual Item GetSetting(Item contextItem, string settingName)
        {
            return this.GetSettingsRoot(contextItem, settingName);
        }

        public virtual Item GetSetting(Item contextItem, ID baseSettingId)
        {
            return this.GetSettingsRoot(contextItem, baseSettingId);
        }

        public virtual UrlOptions GetUrlOptions()
        {
            return new UrlOptions()
            {
                SiteResolving = true,
                LanguageEmbedding = LanguageEmbedding.Never,
                LowercaseUrls = true
            };
        }

        public virtual MediaUrlOptions GetMediaUrlOptions()
        {
            return new MediaUrlOptions()
            {
                AlwaysIncludeServerUrl = true
            };
        }

        private Item GetSettingsRoot(Item contextItem, string settingsName)
        {
            var currentDefinition = this.siteContext.GetSiteDefinition(contextItem);
            if (currentDefinition?.Item == null)
            {
                return null;
            }

            var definitionItem = currentDefinition.Item;
            var settingsFolder = definitionItem.Children[SettingsRootName];
            var settingsRootItem = settingsFolder?.Children.FirstOrDefault(i => i.IsDerived(Templates.SiteSettings.ID) && i.Key.Equals(settingsName.ToLower()));
            return settingsRootItem;
        }

        private Item GetSettingsRoot(Item contextItem, ID baseSettingId)
        {
            var currentDefinition = this.siteContext.GetSiteDefinition(contextItem);
            if (currentDefinition?.Item == null)
            {
                return null;
            }

            var definitionItem = currentDefinition.Item;
            var settingsFolder = definitionItem.Children[SettingsRootName];
            var settingsRootItem = settingsFolder?.Children.FirstOrDefault(i => i.IsDerived(Templates.SiteSettings.ID) && i.IsDerived(baseSettingId));
            return settingsRootItem;
        }
    }
}