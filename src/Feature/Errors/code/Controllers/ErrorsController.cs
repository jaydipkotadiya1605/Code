namespace Sitecore.Feature.Errors.Controllers
{
    using System.Web.Mvc;
    using Sitecore.Feature.Errors.ViewModelBuilders;
    using Sitecore.Foundation.Abstractions.SitecoreContext;
    using Sitecore.Foundation.SitecoreExtensions.Extensions;

    public class ErrorsController : Controller
    {
        private readonly IErrorsViewModelBuilder _errorsViewModelBuilder;
        private readonly ISitecoreContext _sitecoreContext;


        public ErrorsController(
            IErrorsViewModelBuilder errorsViewModelBuilder,
            ISitecoreContext sitecoreContext)
        {
            this._errorsViewModelBuilder = errorsViewModelBuilder;
            this._sitecoreContext = sitecoreContext;
        }
        public ActionResult ItemNotFound()
        {
            var datasource = this._sitecoreContext.DataSourceOrSelf;
            if (datasource == null || !datasource.IsDerived(Templates.ItemNotFound.Id))
            {
                /*
                 * temporary return null
                 * this case should return Foundation.Alert which defined in habitat
                 */
                return null;
            }

            var model = this._errorsViewModelBuilder.GetItemNotFoundModel(datasource);
            return this.View(model);
        }
    }
}