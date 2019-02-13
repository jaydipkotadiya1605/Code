namespace Sitecore.Feature.Enquiry.Extensions
{
    using System.Web.Mvc;
    using Sitecore.Feature.Enquiry.Models;
    using Sitecore.Mvc.Presentation;
    using FrasersContent = Sitecore.Foundation.FrasersContent;
    using Sitecore.Foundation.SitecoreExtensions.Services;

    public class ValidateCaptchaAttribute : ActionFilterAttribute
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
            if (filterContext.RouteData.Values.ContainsKey(Constants.StatusForm)) {
                filterContext.RouteData.Values[Constants.StatusForm] =  Status.Error;
            }
            else
            {
                filterContext.RouteData.Values.Add(Constants.StatusForm, Status.Error);
            }

            if (filterContext.RouteData.Values.ContainsKey(Constants.ErrorMessage))
            {
                filterContext.RouteData.Values[Constants.ErrorMessage] = GoogleValidateCaptchaService.ValidateMessage;
            }
            else
            {
                filterContext.RouteData.Values.Add(Constants.ErrorMessage, GoogleValidateCaptchaService.ValidateMessage);
            }
            filterContext.Result = new ViewResult
            {
                View = pageView
            };
        }
    }
}