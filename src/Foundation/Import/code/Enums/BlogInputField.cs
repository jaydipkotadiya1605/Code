using System.ComponentModel;

namespace Sitecore.Foundation.Import
{
    public enum BlogInputField
    {
        [Description("Category")]
        Category,
        [Description("Title")]
        Title,
        [Description("Thumbnail")]
        Thumbnail,
        [Description("Banner")]
        Banner,
        [Description("Author Name")]
        AuthorName,
        [Description("Post Date/Time")]
        PostDate,
        [Description("Expiry Date/Time")]
        ExpiryDate,
        [Description("Summary")]
        Summary,
        [Description("Body")]
        Body
    }
}