using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sitecore.Foundation.Import
{
    public enum StoreOutputField
    {
        [Description("Store Name")]
        Store,
        [Description("Logo")]
        Logo,
        [Description("Phone Number")]
        PhoneNumber,
        [Description("New Date")]
        NewDate,
        [Description("Store Categories")]
        Category,
        [Description("Description")]
        Description,
        [Description("Unit No")]
        UnitNo,
        [Description("Contact")]
        Contact,
        [Description("Opening Hours")]
        OpeningHrs,
        [Description("Store Offers")]
        StoreOffers,
        [Description("Brands")]
        Brands,
        [Description("Keywords")]
        Keywords,
        [Description("Post Date")]
        UpcomingDate,
        [Description("Expiry Date")]
        ExpiryDate,
        
    }
}