using System;
using System.Linq;
using System.Web.Mvc;

namespace Sitecore.Feature.Contact.Extensions
{
    public static class HtmlExtension
    {
        public static MvcHtmlString SummaryServerError(this HtmlHelper html, ModelStateDictionary state)
        {
            var error = state[nameof(Model.ContactFormVm)]?.Errors?.FirstOrDefault().ErrorMessage;
            return new MvcHtmlString(error);
        }
    }
}