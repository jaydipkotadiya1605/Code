using Newtonsoft.Json;
using Sitecore.Foundation.SitecoreExtensions.Models;
using System;
using System.IO;
using System.Net;
using System.Web.Mvc;

namespace Sitecore.Foundation.SitecoreExtensions.Services
{
    public static class GoogleValidateCaptchaService
    {
        private const string siteVerity = "https://www.google.com/recaptcha/api/siteverify";
        public const string ValidateMessage = "Capcha is not correct";

        public static void ValidCapchaResponse(string secretKey, ActionExecutingContext filterContext, Action<ActionExecutingContext> AddErrorAndRedirectToGetAction)
        {
            const string urlToPost = siteVerity;
            var captchaResponse = filterContext.HttpContext.Request.Form["g-recaptcha-response"];

            if (string.IsNullOrWhiteSpace(captchaResponse)) AddErrorAndRedirectToGetAction(filterContext);

            var validateResult = ValidateFromGoogle(urlToPost, secretKey, captchaResponse);
            if (!validateResult.Success) AddErrorAndRedirectToGetAction(filterContext);
        }

        private static ReCaptchaResponse ValidateFromGoogle(string urlToPost, string secretKey, string captchaResponse)
        {
            var postData = "secret=" + secretKey + "&response=" + captchaResponse;

            var request = (HttpWebRequest)WebRequest.Create(urlToPost);
            request.Method = "POST";
            request.ContentLength = postData.Length;
            request.ContentType = "application/x-www-form-urlencoded";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                streamWriter.Write(postData);

            string result;
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                    result = reader.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<ReCaptchaResponse>(result);
        }
    }
}