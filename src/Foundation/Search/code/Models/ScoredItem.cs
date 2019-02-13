namespace Sitecore.Foundation.Search.Models
{
    using Sitecore.Data.Items;

    public class ScoredItem
    {
        public Item Item { get; set; }

        public float Score { get; set; }

        public string ItemUrl { get; set; }
    }
}