using System.Net.Mail;

namespace Sitecore.Feature.Contact.Model
{
    public class ContactPageSetting
    {
        public string ReceiverName { get; set; }
        public string ReceiverEmail { get; set; }
        public MailMessage EmailMessage { get; set; }
    }
}