using Sitecore.Feature.Enquiry.Models;
using Sitecore.Web;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;

namespace Sitecore.Feature.Enquiry.Extensions
{
    public static class HtmlExtension
    {
        public static MvcHtmlString LowerTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expr, string placeholder, string value)
        {
            var result = new StringBuilder();
            var tag = new TagBuilder("input");
            tag.MergeAttribute("type", "text");
            var lowerPropertyName = ExpressionHelper.GetExpressionText(expr).ToLower();
            tag.MergeAttribute("name", lowerPropertyName);
            tag.MergeAttribute("value", value);
            tag.MergeAttribute("id", lowerPropertyName);
            tag.MergeAttribute("placeholder", placeholder);
            result.Append(tag);
            return new MvcHtmlString(result.ToString());
        }
        public static MvcHtmlString LowerTextAreaFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expr, string value)
        {
            var result = new StringBuilder();
            var tag = new TagBuilder("textarea");
            var lowerPropertyName = ExpressionHelper.GetExpressionText(expr).ToLower();
            tag.MergeAttribute("name", lowerPropertyName);
            tag.MergeAttribute("id", lowerPropertyName);
            tag.InnerHtml = value;
            result.Append(tag);
            return new MvcHtmlString(result.ToString());
        }
        public static MvcHtmlString LowerDropdownFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expr, 
            string placeholder, string[] dataSource, string value)
        {
            var qsWithKey = WebUtil.GetQueryString("type");
            if ("leasing".Equals(qsWithKey, StringComparison.OrdinalIgnoreCase) && string.IsNullOrEmpty(value))
            {
                value = Constants.LeasingEnquiry;
            }
            else if (string.IsNullOrEmpty(value))
            {
                value = Constants.GeneralEnquiry;
            }

            var result = new StringBuilder();
            var tag = new TagBuilder("select");
            var lowerPropertyName = ExpressionHelper.GetExpressionText(expr).ToLower();
            tag.MergeAttribute("name", lowerPropertyName);
            tag.MergeAttribute("id", lowerPropertyName);
            tag.MergeAttribute("class", "valid");
            tag.MergeAttribute("aria-invalid", "false");
            var options = new StringBuilder();
            options.Append($"<option disabled='' selected=''>{placeholder}</option>");
            foreach (var option in dataSource)
            {
                options.Append(option == value ? $"<option selected value='{option}'>{option}</option>" : $"<option value='{option}'>{option}</option>");
            }
            tag.InnerHtml = options.ToString();
            result.Append(tag);
            return new MvcHtmlString(result.ToString());
        }
        public static MvcHtmlString SummaryServerError(this HtmlHelper html, ModelStateDictionary state)
        {
            var error = state[nameof(EnquiryForm)]?.Errors?.FirstOrDefault()?.ErrorMessage;
            return new MvcHtmlString(error);
        }
    }
}