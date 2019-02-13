namespace Sitecore.Feature.Identity.Services
{
    using Sitecore.Feature.Identity.Models;

    public interface IFooterService
    {
        CopyrightModel GetCopyright();
        FooterModel GetFooter();
    }
}