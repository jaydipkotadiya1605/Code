using Sitecore.Foundation.SitecoreExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Enquiry.Models
{
    public class ContactForm
    {
        [Required(ErrorMessage = "Please enter your name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter your email address")]
        [EmailAddress(ErrorMessage = "Email address in invalid format")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter your contact number")]
        public string ContactNo { get; set; }
        [FileSize(5242880, ErrorMessage = "Cannot upload file more than 5Mb")]
        public HttpPostedFileBase UploadFile { get; set; }
    }

    public class EnquiryForm : ContactForm
    {
        public HtmlString TitlePage { get; set; }
        public HtmlString SubTitle { get; set; }
        public string ApiCapchaKey { get; set; }
        public string[] EnquiryTypes { get; set; }
        public string EnquiryTypeSelected { get; set; }
        public string[] EnquirySpaces { get; set; }
        public string EnquirySpaceSelected { get; set; }
        public string Message { get; set; }

        public string ExistingShopName { get; set; }
        public string TradeOrMerchandise { get; set; }
        public string AreaRequired { get; set; }
        public string M_E_Requirements { get; set; }
    }

    public class MailSetting {
        public string ReceiverName { get; set; }
        public string ReceiverEmail { get; set; }
        public string NoReplyEmail { get; set; }
        public string EmailTemplateMessage { get; set; }
        public string Subject { get; set; }
    }
    public enum Status
    {
        Error,
        Success
    }
}