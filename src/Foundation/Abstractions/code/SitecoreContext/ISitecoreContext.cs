namespace Sitecore.Foundation.Abstractions.SitecoreContext
{
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Sitecore.Globalization;
    using Sitecore.Sites;

    public interface ISitecoreContext
    {
        Item Item { get; set; }

        Item DataSourceItem { get; }

        Item DataSourceOrSelf { get; }

        string SiteName { get; }

        Item RootItem { get; }

        Item SiteRoot { get; }

        Language Language { get; }

        Database Database { get; }

        SiteContext Site { get; }

        string ItemNotFoundPage { get; }

        string ServerErrorPage { get; }
    }
}