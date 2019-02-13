namespace Sitecore.Foundation.SitecoreExtensions.Models
{
    using Sitecore.Data.Items;

    public class ImageItem
    {
        public Item Item { get; set; }
        public string Src { get; set; }
        public string Alt { get; set; }
        public string Width { get; set; }
        public string Heigth { get; set; }
    }
}