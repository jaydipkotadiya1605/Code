using System.ComponentModel;

namespace Sitecore.Foundation.Import
{
    public enum BannerOutputField
    {
        [Description("Title")]
        Title,
        [Description("Image")]
        Image,
        [Description("Summary")]
        Summary,
        [Description("Link")]
        Link,
        [Description("Category")]
        Category,
        [Description("Post Date")]
        PostDate,
        [Description("Expiry Date")]
        ExpiryDate,
        [Description("Display on main site")]
        ShowInMain,
        [Description("Display on malls")]
        ShowInMalls
    }
}