namespace Sitecore.Feature.Identity.Models
{
    using System.Web;
    using Sitecore.Data.Items;

    public class FooterModel
    {
        public HtmlString Logo { get; set; }
        public Item Menu { get; set; }
        public SocialItems SocialIcons { get; set; }
        public bool IncludeHeaderSocialIcons { get; set; }
    }

    public class MobileFooterModel
    {
        public string Name { get; set; }
        public string ItemUrl { get; set; }
        public bool IsExternalLink { get; set; }
    }
}