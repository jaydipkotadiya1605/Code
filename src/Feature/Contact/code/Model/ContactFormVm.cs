namespace Sitecore.Feature.Contact.Model
{
    using System.Web;
    using System.ComponentModel.DataAnnotations;
    using Sitecore.Mvc.Presentation;
    using Sitecore.Feature.Contact.Extensions;
    using Sitecore.Foundation.SitecoreExtensions.Attributes;

    public class ContactFormVm : RenderingModel
    {
        [Required(ErrorMessage = "Please enter your name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter your email address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter your contact number")]
        public string ContactNo { get; set; }
        [Required(ErrorMessage = "Please enter your message")]
        public string Message { get; set; }
        [FileSize(5242880, ErrorMessage = "Cannot upload file more than 5Mb")]
        public HttpPostedFileBase UploadFile { get; set; }
    }
    public enum Status
    {
        Error,
        Success
    }
}