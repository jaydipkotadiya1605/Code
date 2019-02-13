using System.ComponentModel;

namespace Sitecore.Foundation.Import
{
    public enum ArticleOutputField
    {
        [Description("Title")]
        Title,
        [Description("Thumbnail")]
        Thumbnail,
        [Description("Banner")]
        Banner,
        [Description("Post Date")]
        PostDate,
        [Description("Expiry Date")]
        ExpiryDate,
        [Description("Summary")]
        Summary,
        [Description("Description")]
        Description,
        [Description("Store")]
        Store,
        [Description("Start Date")]
        StartDate,
        [Description("End Date")]
        EndDate,
        [Description("Display on malls")]
        ShowInMalls,
        [Description("Category")]
        Category
    }
}