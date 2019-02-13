namespace Sitecore.Feature.Contact.Extensions
{
    using System.Web.Mvc;
    using Sitecore.Feature.Contact.Model;
    using Sitecore.Mvc.Presentation;
    using FrasersContent = Sitecore.Foundation.FrasersContent;
    using Sitecore.Foundation.SitecoreExtensions.Services;

    public class ValidateGoogleCaptchaAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var secretKey = Context.Item.Fields[FrasersContent.Templates.ContactFormMainSite.Fields.GoogleCapchaPrivateKey].Value;
            GoogleValidateCaptchaService.ValidCapchaResponse(secretKey, filterContext, AddErrorAndRedirectToGetAction);
            base.OnActionExecuting(filterContext);
        }

        private static void AddErrorAndRedirectToGetAction(ActionExecutingContext filterContext)
        {
            IView pageView = PageContext.Current.PageView;
            filterContext.Controller.ViewData["Status"] = Status.Error;
            filterContext.Controller.ViewData.ModelState.AddModelError(nameof(ContactFormVm), GoogleValidateCaptchaService.ValidateMessage);
            filterContext.Result = new ViewResult
            {
                View = pageView,
                ViewData = filterContext.Controller.ViewData,
                TempData = filterContext.Controller.TempData,
            };
        }
    }
}