using System.Collections.Generic;

namespace Sitecore.Feature.Identity.Models
{
    public class SocialItem
    {
        public string Name { get; set; }
        public string IconUrl { get; set; }
        public string MobileIconUrl { get; set; }
        public string Link { get; set; }
        public string Css { get; set; }
        public string IconPostText { get; set; }
    }

    public class SocialItems
    {
        public IList<SocialItem> Items { get; set; }
    }
}