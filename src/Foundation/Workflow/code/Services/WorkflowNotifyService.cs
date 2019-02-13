using System.Collections.Generic;
using System.Net.Mail;
using Sitecore.Foundation.Workflow.Helpers;
using Sitecore.Foundation.Workflow.Models;
using Sitecore.Data.Items;
using Sitecore.Security.Accounts;
using System.Web;
using Sitecore.Foundation.DependencyInjection;
using Sitecore.Globalization;

namespace Sitecore.Foundation.Workflow.Services
{
    [Service(typeof(IWorkflowNotifyService))]
    public class WorkflowNotifyService : IWorkflowNotifyService
    {
        public void SendEmailNotifications(IEnumerable<User> receivers,string from, Item contentItem, string comment, Item emailTemplateItem)
        {
            var subject = $"{emailTemplateItem.Fields[Templates.EmailTemplate.Fields.Subject].Value} {contentItem.Name}";
            var bodyTemplateEmail = emailTemplateItem.Fields[Templates.EmailTemplate.Fields.Message].Value;
            if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(bodyTemplateEmail))
            {
                throw new Exceptions.ClientAlertException(Translate.Text("Setting Email in workflow is not correct"));
            }

            foreach (var receiver in receivers)
            {
                var to = receiver.Profile.Email;
                if (string.IsNullOrEmpty(to)) { continue; }
                var message = this.GetMailMessage(receiver, contentItem, subject, comment, bodyTemplateEmail);
                message.From = new MailAddress(from);
                message.To.Add(to);
                Sitecore.MainUtil.SendMail(message);
            }
        }

        private MailMessage GetMailMessage(User receiver, Item contentItem, string subject, string comment, string bodyTemplate)
        {
            string previewUrl = GeneratePreviewUrl(contentItem);
            var bodyData = new BodyEmail
            {
                ReceiverName = (!string.IsNullOrEmpty(receiver.Profile.FullName)) ? receiver.Profile.FullName : receiver.Profile.UserName,
                Comment = comment,
                ItemName = contentItem.Name,
                ItemUrl = string.Format(Constants.ItemUrlFormat, previewUrl)
            };
            var bodyMessage = bodyTemplate.ToEmail(bodyData);
            var result = new MailMessage
            {
                IsBodyHtml = true,
                Subject = subject,
                Body = bodyMessage
            };
            return result;
        }
        private static string GeneratePreviewUrl(Item contentItem)
        {
            bool hasPresentation = WorkflowHelper.DoesItemHasPresentationDetails(contentItem.ID.Guid.ToString());
            string previewUrl;

            // Generate preview link
            if (hasPresentation)
            {
                previewUrl = string.Format(Constants.PreviewUrlHasPresentation,
                HttpContext.Current.Request.Url.Scheme,
                HttpContext.Current.Request.Url.Host,
                contentItem.ID.Guid.ToString().ToUpper(),
                contentItem.Language.Name);
            }
            else
            {
                previewUrl = string.Format(Constants.PreviewUrlNoPresentation,
                HttpContext.Current.Request.Url.Scheme,
                HttpContext.Current.Request.Url.Host,
                contentItem.ID.Guid.ToString().ToUpper(),
                contentItem.Language.Name);
            }

            return previewUrl;
        }
    }
}