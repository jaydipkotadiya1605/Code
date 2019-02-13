namespace Sitecore.Foundation.Abstractions.SitecoreContext
{
    using DependencyInjection;
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Sitecore.Globalization;
    using Sitecore.Mvc.Presentation;
    using Sitecore.Sites;

    [Service(typeof(ISitecoreContext))]
    public class SitecoreContext : ISitecoreContext
    {
        public Item Item
        {
            get
            {
                if (Context.Item != null)
                {
                    return Context.Item;
                }

                return null;
            }
            set
            {
                Context.Item = value;
            } 
        }

        public Item DataSourceItem
        {
            get
            {
                if (RenderingContext.Current.Rendering != null)
                {
                    var dataSourceId = RenderingContext.Current.Rendering.DataSource;

                    return ID.IsID(dataSourceId) ? this.Database.GetItem(dataSourceId) : null;
                }

                return null;
            }
        }

        public Item DataSourceOrSelf
        {
            get
            {
                if (this.DataSourceItem != null)
                {
                    return this.DataSourceItem;
                }

                return this.Item;
            }
        }

        public Item FormItem
        {
            get
            {
                if (RenderingContext.Current.Rendering != null)
                {
                    var formId = RenderingContext.Current.Rendering.Parameters["FormId"];

                    return ID.IsID(formId) ? this.Database.GetItem(formId) : null;
                }

                return null;
            }
        }

        public string SiteName => Context.Site.Name;

        public Item RootItem => this.Database.GetRootItem();

        public Item SiteRoot
        {
            get
            {
                var startPath = Context.Site.StartPath;
                return this.Database.GetItem(startPath);
            }
        }

        public Language Language => Context.Language;

        public Database Database => Context.Database;

        public SiteContext Site => Context.Site;

        public string ItemNotFoundPage => Context.Site.Properties[Constants.ItemNotFoundPage];

        public string ServerErrorPage => Context.Site.Properties[Constants.ServerErrorPage];
    }
}