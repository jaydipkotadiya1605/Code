using System;

namespace Sitecore.Foundation.Assets.Configuration
{
    public static class Configuration
    {
        public static bool IsUseMinifyJavascript()
        {
           return Sitecore.Configuration.Settings.GetBoolSetting("Sitecore.Foundation.Assets.IsUseMinifyJs", true);
        }
    }
}