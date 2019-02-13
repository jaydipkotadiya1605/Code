namespace Sitecore.Feature.Errors.ViewModelBuilders
{
    using Sitecore.Data.Items;
    using Sitecore.Feature.Errors.Models;
    using Sitecore.Foundation.DependencyInjection;
    using Sitecore.Foundation.SitecoreExtensions.Extensions;

    [Service(typeof(IErrorsViewModelBuilder))]
    public class ErrorsViewModelBuilder : IErrorsViewModelBuilder
    {
        public ItemNotFoundModel GetItemNotFoundModel(Item item)
        {
            return new ItemNotFoundModel
            {
                Title = item.Field(Templates.ItemNotFound.Fields.Title),
                Subtitle = item.Field(Templates.ItemNotFound.Fields.Subtitle)
            };
        }
    }
}