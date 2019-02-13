namespace Sitecore.Feature.Sitemap.ViewModelBuilders
{
    using Sitecore.Data.Items;
    using Sitecore.Foundation.DependencyInjection;
    using Sitecore.Foundation.SitecoreExtensions.Extensions;

    [Service(typeof(IRobotsViewModelBuilder))]
    public class RobotsViewModelBuilder : IRobotsViewModelBuilder
    {
        public string GetRobotsContent(Item item)
        {
            return item.GetString(Templates.Robots.Fields.Content);
        }
    }
}