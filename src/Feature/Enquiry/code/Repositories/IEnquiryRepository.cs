using Sitecore.Feature.Enquiry.Models;

namespace Sitecore.Feature.Enquiry.Repositories
{
    public interface IEnquiryRepository
    {
        EnquiryForm RenderEnquiryForm(EnquiryForm form);
        string IsValidForm(EnquiryForm form);
        string SendGeneralInquiry(EnquiryForm form);
        string SendLeasingInquiry(EnquiryForm form);
    }
}
