using System.ComponentModel;

namespace Sitecore.Foundation.Import
{
    public enum ArticleCategory
    {
        [Description("Offers")]
        Offers,
        [Description("Contests")]
        Contests,
        [Description("Special Event")]
        SpecialEvent,
        [Description("Promotions")]
        Promotions
    }
}