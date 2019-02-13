using Sitecore.Data;
using Sitecore.Foundation.Workflow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Foundation.Workflow.Helpers
{
    public static class NotifyTemplateBuilder
    {
        public static string ToEmail(this string emailTemplate, BodyEmail bodyData)
            => emailTemplate.ReplacePlaceHodler(new Dictionary<string, string>
            {
                { Constants.EmailToken.Receiver, bodyData.ReceiverName },
                { Constants.EmailToken.SubmitComment, bodyData.Comment },
                { Constants.EmailToken.ItemName, bodyData.ItemName } ,
                { Constants.EmailToken.ItemUrl, bodyData.ItemUrl } ,
                { Constants.EmailToken.CurrentActionUser, Context.User.Name }
            });
        public static string ReplacePlaceHodler(this string replacingString, Dictionary<string, string> replacePair)
        {
            foreach (KeyValuePair<string, string> entry in replacePair)
            {
                replacingString = replacingString.Replace(entry.Key, entry.Value ?? Constants.NoComment);
            }
            return replacingString;
        }
    }
}