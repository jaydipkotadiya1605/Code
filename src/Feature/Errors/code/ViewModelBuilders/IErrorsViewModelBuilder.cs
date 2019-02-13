namespace Sitecore.Feature.Errors.ViewModelBuilders
{
    using Sitecore.Data.Items;
    using Sitecore.Feature.Errors.Models;

    public interface IErrorsViewModelBuilder
    {
        ItemNotFoundModel GetItemNotFoundModel(Item item);
    }
}