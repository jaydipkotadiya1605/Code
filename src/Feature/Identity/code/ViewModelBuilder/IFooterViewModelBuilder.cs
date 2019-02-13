namespace Sitecore.Feature.Identity.ViewModelBuilder
{
    using Sitecore.Data.Items;
    using Sitecore.Feature.Identity.Models;

    public interface IFooterViewModelBuilder
    {
        FooterModel GetFooterModel(Item item);
        CopyrightModel GetCopyrightModel(Item item);
    }
}