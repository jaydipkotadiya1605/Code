﻿namespace Sitecore.Foundation.SitecoreExtensions.Attributes
{
    using Sitecore.Mvc.Presentation;
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Web.Mvc;


    public class ValidateRenderingIdAttribute : ActionMethodSelectorAttribute
    {
        internal const string FormUniqueid = "uid";

        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            var ignoreCase = StringComparison.InvariantCultureIgnoreCase;

            var httpRequest = controllerContext.HttpContext.Request;
            var isWebFormsForMarketersRequest = httpRequest.Form.AllKeys
              .Any(key => key.StartsWith("wffm", ignoreCase) && key.EndsWith("Id", ignoreCase));

            if (isWebFormsForMarketersRequest)
            {
                return false;
            }
            string renderingId = httpRequest.Form[FormUniqueid];
            if (!httpRequest.GetHttpMethodOverride().Equals(HttpVerbs.Post.ToString(), ignoreCase) || string.IsNullOrEmpty(renderingId))
            {
                return true;
            }

            var renderingContext = RenderingContext.CurrentOrNull;
            if (renderingContext == null)
            {
                return false;
            }

            Guid id;
            return Guid.TryParse(renderingId, out id) && id.Equals(renderingContext.Rendering.UniqueId);
        }
    }
}