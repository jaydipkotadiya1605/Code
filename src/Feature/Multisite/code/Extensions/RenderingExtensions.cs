using Sitecore.Mvc.Presentation;

namespace Sitecore.Feature.Multisite.Extensions
{
    public static class RenderingExtensions
    {
        public static string GetListClass([NotNull] this Rendering rendering)
        {
            return rendering.Parameters[Constants.ListParametters.ListClass];
        }
    }
}