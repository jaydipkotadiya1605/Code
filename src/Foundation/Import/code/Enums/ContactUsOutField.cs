using System.ComponentModel;

namespace Sitecore.Foundation.Import
{
    public enum ContactUsOutField
    {
        [Description("Address")]
        Address,
        [Description("GPS Lattitude")]
        Latitude,
        [Description("GPS Longitude")]
        Longitude,
        [Description("Telephone")]
        Telephone,
        [Description("Opening Hours")]
        OpeningHrs,
        [Description("Details")]
        Details,
        [Description("Photo")]
        Photo,
        [Description("Leasing Resources")]
        LeasingResources,
    }
}