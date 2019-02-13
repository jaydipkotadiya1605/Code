using Newtonsoft.Json;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using FrasersContent = Sitecore.Foundation.FrasersContent;

namespace Sitecore.Feature.WebApi.Models
{
    public class Banner
    {
        [JsonProperty(Order = 1, PropertyName = "id")]
        public ID Id { get; set; }
        [JsonProperty(Order = 2, PropertyName = "title")]
        public string Title { get; set; }
        [JsonProperty(Order = 3, PropertyName = "banner")]
        public string Image { get; set; }
        [JsonProperty(Order = 4, PropertyName = "banner_mobile")]
        public string MobileImage { get; set; }
        [JsonProperty(Order = 5, PropertyName = "link")]
        public string Link { get; set; }
        
        public Banner()
        {
            this.Id = null;
            this.Title = null;
            this.Image = null;
            this.MobileImage = null;
            this.Link = null;
        }

        public Banner(Item item)
        {
            if (item == null || !item.IsDerived(FrasersContent.Templates.Banner.ID))
            {
                return;
            }

            this.Id = item.ID;
            this.Title = item.Fields[FrasersContent.Templates.Banner.Fields.Title].Value;

            // Get banner image
            this.Image = item.ImageUrl(FrasersContent.Templates.Banner.Fields.Image, Constants.DefaultMediaUrllOptions);
            this.MobileImage = item.ImageUrl(FrasersContent.Templates.Banner.Fields.MobileImage, Constants.DefaultMediaUrllOptions);

            // Get link
            this.Link = item.LinkFieldUrl(FrasersContent.Templates.Banner.Fields.Link);
        }
    }
}