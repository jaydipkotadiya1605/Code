namespace Sitecore.Feature.Social.Services
{
    using Sitecore.Data.Items;
    using Sitecore.Foundation.Abstractions.SitecoreContext;
    using Sitecore.Foundation.DependencyInjection;
    using Sitecore.Foundation.SitecoreExtensions.Extensions;
    using Sitecore.Links;
    using System;

    [Service(typeof(ISocialService))]
    public class SocialService : ISocialService
    {
        private const string DefaultType = "article";
        private ISitecoreContext SitecoreContext { get; }

        public SocialService(ISitecoreContext sitecoreContext)
        {
            this.SitecoreContext = sitecoreContext;
        }

        public Models.OpenGraphMetadata GetOpenGraphMetadata()
        {
            if (!this.SitecoreContext.Item.IsDerived(Templates.OpenGraph.ID)) return null;

            var options = LinkManager.GetDefaultUrlOptions();
            options.AlwaysIncludeServerUrl = true;

            var metadata = new Models.OpenGraphMetadata()
            {
                Title = this.GetStringField(Templates.OpenGraph.Fields.Title, (item) => item.Fields[Templates.OpenGraph.Fields.TitleName] != null ? item.Fields[Templates.OpenGraph.Fields.TitleName].Value : string.Empty),
                Description = this.GetStringField(Templates.OpenGraph.Fields.Description, (item) => item.Fields[Templates.OpenGraph.Fields.DescriptionName] != null ? item.Fields[Templates.OpenGraph.Fields.DescriptionName].Value : string.Empty),
                Type = this.GetStringField(Templates.OpenGraph.Fields.Type, (item) => DefaultType),
                ItemUrl =  this.SitecoreContext.Item.Url(options),
                ImageUrl = this.GetImageField(Templates.OpenGraph.Fields.Image, (item) => {
                    if (item.Fields[Templates.OpenGraph.Fields.ImageName] == null)
                        return string.Empty;

                    var imgField = ((Data.Fields.ImageField) item.Fields[Templates.OpenGraph.Fields.ImageName]);
                    return Resources.Media.MediaManager.GetMediaUrl(imgField.MediaItem, new Resources.Media.MediaUrlOptions
                    {
                        AlwaysIncludeServerUrl = true,
                        AbsolutePath = true
                    });
                })
            };
            return metadata;
        }

        private string GetStringField(Data.ID fieldTemplateId, Func<Item, string> getDefaultValue)
        {
            var value = this.SitecoreContext.Item.GetString(fieldTemplateId);
            if (string.IsNullOrEmpty(value))
            {
                value = getDefaultValue(this.SitecoreContext.Item);
            }
            return value;
        }

        private string GetImageField(Data.ID fieldTemplateId, Func<Item, string> getDefaultValue)
        {
            if (this.SitecoreContext.Item.Fields[fieldTemplateId].HasValue)
            {
                return this.SitecoreContext.Item.ImageUrl(fieldTemplateId, new Resources.Media.MediaUrlOptions
                {
                    AlwaysIncludeServerUrl = true,
                    AbsolutePath = true
                });
            }
            return getDefaultValue(this.SitecoreContext.Item);
        }
    }
}