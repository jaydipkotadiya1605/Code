namespace Sitecore.Feature.Sitemap.ViewModelBuilders
{
    using Sitecore.Data.Items;

    public interface IRobotsViewModelBuilder
    {
        string GetRobotsContent(Item item);
    }
}