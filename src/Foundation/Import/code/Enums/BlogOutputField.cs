using System.ComponentModel;

namespace Sitecore.Foundation.Import
{
    public enum BlogOutputField
    {
        [Description("Category")]
        Category,
        [Description("Title")]
        Title,
        [Description("Thumbnail")]
        Thumbnail,
        [Description("Banner")]
        Banner,
        [Description("Author")]
        Author,
        [Description("Post Date")]
        PostDate,
        [Description("Expiry Date")]
        ExpiryDate,
        [Description("Summary")]
        Summary,
        [Description("Body")]
        Body
    }
}