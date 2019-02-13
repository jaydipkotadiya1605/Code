namespace Sitecore.Foundation.SitecoreExtensions.Repositories
{
    using Sitecore.Mvc.Presentation;

    public interface IRenderingPropertiesRepository
    {
        T Get<T>(Rendering rendering);
        T GetExt<T>(Rendering rendering);
    }
}