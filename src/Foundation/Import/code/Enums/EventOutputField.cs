using System.ComponentModel;

namespace Sitecore.Foundation.Import
{
    public enum EventOutputField
    {
        [Description("Title")]
        Title,
        [Description("Post Date")]
        PostDate,
        [Description("Store")]
        Store,
        [Description("All Store")]
        AllStore,
        [Description("Keywords")]
        Keywords,
        [Description("Start Date")]
        StartDate,
        [Description("End Date")]
        EndDate,
        [Description("Image")]
        Image,
        [Description("Summary")]
        Summary,
        [Description("Event Type")]
        EventType,
        [Description("Display on malls")]
        ShowInMalls,
        [Description("Thumbnail")]
        Thumbnail,
        [Description("Description")]
        Description
    }
}