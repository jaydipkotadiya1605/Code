
using Sitecore.Data.Items;

namespace Sitecore.Feature.Banner.Models
{
    public class BannerItem
    {
        public Item Item { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string Summary { get; set; }
        public string Link { get; set; }
    }
}