using Sitecore.Foundation.FrasersContent;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;

namespace Sitecore.Foundation.SitecoreExtensions.Extensions
{
    public static class HtmlExtension
    {
        public static MvcHtmlString LowerTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expr, string placeholder)
        {
            StringBuilder result = new StringBuilder();
            TagBuilder tag = new TagBuilder("input");
            var lowerPropertyName = ExpressionHelper.GetExpressionText(expr).ToLower();
            var name = ExpressionHelper.GetExpressionText(expr);
            string fullHtmlFieldName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            string propertyValue = (string)html.GetModelStateValue(fullHtmlFieldName, typeof(string));
            //Generate tag attributes
            tag.MergeAttribute("type", "text");
            tag.MergeAttribute("name", lowerPropertyName);
            tag.MergeAttribute("id", lowerPropertyName);
            tag.MergeAttribute("placeholder", placeholder);
            if (!string.IsNullOrEmpty(propertyValue))
            {
                tag.MergeAttribute("value", propertyValue);
            }
            result.Append(tag.ToString());
            return new MvcHtmlString(result.ToString());
        }
        public static MvcHtmlString LowerTextAreaFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expr)
        {
            StringBuilder result = new StringBuilder();
            TagBuilder tag = new TagBuilder("textarea");
            var lowerPropertyName = ExpressionHelper.GetExpressionText(expr).ToLower();
            var name = ExpressionHelper.GetExpressionText(expr);
            string fullHtmlFieldName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            string propertyValue = (string)html.GetModelStateValue(fullHtmlFieldName, typeof(string));
            tag.MergeAttribute("name", lowerPropertyName);
            tag.MergeAttribute("id", lowerPropertyName);
            if (!string.IsNullOrEmpty(propertyValue))
            {
                tag.InnerHtml = propertyValue;
            }
            result.Append(tag.ToString());
            return new MvcHtmlString(result.ToString());
        }
        public static object GetModelStateValue(this HtmlHelper html, string key, Type destinationType)
        {
            ModelState modelState;
            if (html.ViewData.ModelState.TryGetValue(key, out modelState) && modelState.Value != null)
            {
                return modelState.Value.ConvertTo(destinationType, null);
            }
            return null;
        }

        public static MvcHtmlString RenderAddThisTokenScript()
        {
            var script = string.Empty;
            var rootItem = Context.Site.GetRootItem();

            if (rootItem != null)
            {
                var token = rootItem.GetString(Templates.AddThisSocialToken.ID);
                if (!string.IsNullOrEmpty(token))
                    script = $"<script type='text/javascript' src='//s7.addthis.com/js/300/addthis_widget.js#pubid=ra-{token}'></script>";
            }
            return new MvcHtmlString(script);
        }
    }
}