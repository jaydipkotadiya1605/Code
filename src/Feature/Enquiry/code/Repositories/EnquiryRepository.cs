namespace Sitecore.Feature.Enquiry.Repositories
{
    using Sitecore.Data.Items;
    using Sitecore.Diagnostics;
    using Sitecore.Feature.Enquiry.Models;
    using Sitecore.Foundation.Abstractions.SitecoreContext;
    using Sitecore.Foundation.DependencyInjection;
    using Sitecore.Foundation.SitecoreExtensions.Extensions;
    using Sitecore.Foundation.Workflow.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using FrasersContent = Sitecore.Foundation.FrasersContent;

    [Service(typeof(IEnquiryRepository))]
    public class EnquiryRepository : IEnquiryRepository
    {
        private readonly ISitecoreContext sitecoreContext;
        public EnquiryRepository(ISitecoreContext sitecoreContext)
        {
            this.sitecoreContext = sitecoreContext;
        }

        public EnquiryForm RenderEnquiryForm(EnquiryForm form)
        {
            if (form == null)
            {
                form = new EnquiryForm();
            }
            form.EnquiryTypes = this.GetEnquiryTypes();
            form.EnquirySpaces = this.GetLeasingTypes();
            form.ApiCapchaKey = this.sitecoreContext.Item.GetString(FrasersContent.Templates.ContactFormMainSite.Fields.GoogleCapchaPublicKey);
            form.TitlePage = this.sitecoreContext.Item.Field(FrasersContent.Templates.ContactFormMainSite.Fields.ContactPageTitle);
            form.SubTitle = this.sitecoreContext.Item.Field(FrasersContent.Templates.ContactFormMainSite.Fields.ContactPageSubTitle);
            return form;
        }

        public string IsValidForm(EnquiryForm form)
        {
            var message = string.Empty;
            if (string.IsNullOrEmpty(form.EnquiryTypeSelected.Trim()))
            {
                message = "Please select an enquiry type";
            }
            else {
                if (Constants.GeneralEnquiry.Equals(form.EnquiryTypeSelected, StringComparison.OrdinalIgnoreCase))
                {
                    if (string.IsNullOrEmpty(form.Message.Trim()))
                    {
                        message = "Please enter your message";
                    }
                } else if (Constants.LeasingEnquiry.Equals(form.EnquiryTypeSelected, StringComparison.OrdinalIgnoreCase) && string.IsNullOrEmpty(form.EnquirySpaceSelected.Trim()))
                {
                    message = "Please select a space";
                }
            }

            return message;
        }

        public string SendGeneralInquiry(EnquiryForm form)
        {
            return this.SendInquiry((inquiryPageItem) => new MailSetting()
            {
                EmailTemplateMessage = WebUtility.HtmlDecode(inquiryPageItem.Fields[Foundation.Workflow.Templates.EmailTemplate.Fields.Message].Value),
                NoReplyEmail = inquiryPageItem.Fields[FrasersContent.Templates.ContactFormMainSite.Fields.NoReplyEmail].Value,
                ReceiverEmail = inquiryPageItem.Fields[FrasersContent.Templates.ContactFormMainSite.Fields.ReceiverEmail].Value,
                ReceiverName = inquiryPageItem.Fields[FrasersContent.Templates.ContactFormMainSite.Fields.ReceiverName].Value,
                Subject = inquiryPageItem.Fields[Foundation.Workflow.Templates.EmailTemplate.Fields.Subject].Value
            }, (mailSetting) => this.BuildGeneralInquiryTemplate(mailSetting, form), form);
        }


        public string SendLeasingInquiry(EnquiryForm form)
        {
            return this.SendInquiry((inquiryPageItem) => {
                return new MailSetting() {
                    EmailTemplateMessage = WebUtility.HtmlDecode(inquiryPageItem.Fields[Templates.EnquiryTemplate.Fields.Message].Value),
                    NoReplyEmail = inquiryPageItem.Fields[FrasersContent.Templates.ContactFormMainSite.Fields.NoReplyEmail].Value,
                    ReceiverEmail = inquiryPageItem.Fields[FrasersContent.Templates.ContactFormMainSite.Fields.ReceiverEmail].Value,
                    ReceiverName = inquiryPageItem.Fields[FrasersContent.Templates.ContactFormMainSite.Fields.ReceiverName].Value,
                    Subject = inquiryPageItem.Fields[Templates.EnquiryTemplate.Fields.Subject].Value
                };
            }, (mailSetting) => this.BuildLeasingInquiryTemplate(mailSetting, form), form);
        }

        private Dictionary<string, string> BuildGeneralInquiryTemplate(MailSetting mailSetting, EnquiryForm form) {

            return new Dictionary<string, string> {
                                                            { Foundation.Workflow.Constants.EmailToken.Receiver, mailSetting.ReceiverName },
                                                            { $"[{nameof(form.Name)}]", form.Name },
                                                            { $"[{nameof(form.Email)}]",  form.Email },
                                                            { $"[{nameof(form.ContactNo)}]", form.ContactNo },
                                                            { $"[{nameof(form.Message)}]", form.Message }
                                                    };
        }

        private Dictionary<string, string> BuildLeasingInquiryTemplate(MailSetting mailSetting, EnquiryForm form)
        {
            var inquiryTemplate = this.BuildGeneralInquiryTemplate(mailSetting, form);
            inquiryTemplate.Add(nameof(form.EnquirySpaceSelected), form.EnquirySpaceSelected);
            inquiryTemplate.Add(nameof(form.ExistingShopName), form.ExistingShopName);
            inquiryTemplate.Add(nameof(form.TradeOrMerchandise), form.TradeOrMerchandise);
            inquiryTemplate.Add(nameof(form.AreaRequired), form.AreaRequired);
            inquiryTemplate.Add(nameof(form.M_E_Requirements), form.M_E_Requirements);
            return inquiryTemplate;
        }

        private string SendInquiry(Func<Item, MailSetting> generateMailSetting, Func<MailSetting, Dictionary<string, string>> generateMappingTemplate, EnquiryForm form)
        {
            string errorMessage = string.Empty;
            try
            {
                var mailSetting = generateMailSetting(Context.Item);
                var emailMessage = new MailMessage
                {
                    IsBodyHtml = true,
                    From = new MailAddress(mailSetting.NoReplyEmail),
                    Subject = mailSetting.Subject,
                    Body = mailSetting.EmailTemplateMessage.ReplacePlaceHodler(generateMappingTemplate(mailSetting))
                };
                emailMessage.To.Add(mailSetting.ReceiverEmail);
                if (form.UploadFile != null && form.UploadFile.ContentLength > 0)
                {
                    var attachment = new Attachment(form.UploadFile.InputStream, form.UploadFile.FileName);
                    emailMessage.Attachments.Add(attachment);
                }
                if (emailMessage.To.Count > 0)
                {
                    MainUtil.SendMail(emailMessage);
                    Log.Info($"Sending Mail to: {emailMessage.To}", this);
                }
            }
            catch (Exception ex)
            {
                errorMessage = "Some thing go wrong with server";
                Log.Error($"Sending Mail Error: {ex.Message}", this);
            }
            return errorMessage;
        }

        private string[] GetEnquiryTypes() {
            var resources = this.sitecoreContext.Database.GetItem(Templates.EnquiryResources.ID);
            return resources.Children.Where(x => x.IsDerived(Templates.EnquiryType.ID)).Select(x => x.DisplayName).ToArray();
        }

        private string[] GetLeasingTypes()
        {
            var resources = this.sitecoreContext.Database.GetItem(Templates.LeasingResources.ID);
            return resources.Children.Where(x => x.IsDerived(Templates.LeasingType.ID)).Select(x => x.DisplayName).ToArray();
        }
    }
}