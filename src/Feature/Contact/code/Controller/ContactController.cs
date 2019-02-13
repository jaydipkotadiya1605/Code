namespace Sitecore.Feature.Contact.Controller
{
    using Sitecore.Feature.Contact.Model;
    using System.Web.Mvc;
    using Sitecore.Mvc.Presentation;
    using Sitecore.Mvc.Controllers;
    using System.Net;
    using System.Net.Mail;
    using Sitecore.Diagnostics;
    using System.Collections.Generic;
    using Sitecore.Foundation.Workflow.Helpers;
    using System;
    using Sitecore.Mvc.Configuration;
    using Sitecore.Feature.Contact.Extensions;
    using FrasersContent = Sitecore.Foundation.FrasersContent;

    public class ContactController : SitecoreController
    {

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateGoogleCaptcha]
        public ActionResult Submit(ContactFormVm vm)
        {
            IView pageView = PageContext.Current.PageView;
            var contactPageItem = Context.Item;
            if (ModelState.IsValid)
            {
                try
                {
                    //Mail Setting
                    var receiverName = contactPageItem.Fields[FrasersContent.Templates.ContactFormMainSite.Fields.ReceiverName].Value;
                    var receiverEmail = contactPageItem.Fields[FrasersContent.Templates.ContactFormMainSite.Fields.ReceiverEmail].Value;
                    var noReplyEmail = contactPageItem.Fields[FrasersContent.Templates.ContactFormMainSite.Fields.NoReplyEmail].Value;
                    // Mail Template
                    var subject = contactPageItem.Fields[Foundation.Workflow.Templates.EmailTemplate.Fields.Subject].Value;
                    var emailTemplateMessage = WebUtility.HtmlDecode(contactPageItem.Fields[Foundation.Workflow.Templates.EmailTemplate.Fields.Message].Value);

                    var emailMessage = new MailMessage
                    {
                        IsBodyHtml = true,
                        From = new MailAddress(noReplyEmail),
                        Subject = subject,
                        Body = NotifyTemplateBuilder.ReplacePlaceHodler(
                                       emailTemplateMessage,
                                       new Dictionary<string, string> {
                                                            { Foundation.Workflow.Constants.EmailToken.Receiver, receiverName },
                                                            { $"[{nameof(vm.Name)}]", vm.Name },
                                                            { $"[{nameof(vm.Email)}]",  vm.Email },
                                                            { $"[{nameof(vm.ContactNo)}]", vm.ContactNo },
                                                            { $"[{nameof(vm.Message)}]", vm.Message }
                                                        }
                                       )
                    };
                    emailMessage.To.Add(receiverEmail);
                    if (vm.UploadFile != null && vm.UploadFile.ContentLength > 0)
                    {
                        var attachment = new Attachment(vm.UploadFile.InputStream, vm.UploadFile.FileName);
                        emailMessage.Attachments.Add(attachment);
                    }
                    if (emailMessage.To.Count > 0)
                    {
                        MainUtil.SendMail(emailMessage);
                        Log.Info($"Sending Mail to: {emailMessage.To}", this);
                    }

                    return this.RedirectToRoute(MvcSettings.SitecoreRouteName, new { status = Status.Success });
                }
                catch (Exception ex)
                {
                    ViewData.Add("Status", Status.Error);
                    ModelState.AddModelError(nameof(ContactFormVm), "Some thing go wrong with server");
                    Log.Error($"Sending Mail Error: {ex.Message}", this);
                }
            }
            return this.View(pageView, vm);
        }
    }
}