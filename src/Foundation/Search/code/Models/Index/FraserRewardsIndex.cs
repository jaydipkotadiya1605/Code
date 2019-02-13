namespace Sitecore.Foundation.Search.Models.Index
{
    using Sitecore.ContentSearch;

    public class FraserRewardsIndex :SchedulableItemIndex
    {
        [IndexField("title")]
        public string Title { get; set; }

        [IndexField("title_lowercase")]
        public string TitleLowercase { get; set; }

        [IndexField("summary")]
        public string Summary { get; set; }

        [IndexField("description")]
        public string Description { get; set; }

        [IndexField("booking_id")]
        public string BookingId { get; set; }

        [IndexField("author")]
        public string Author { get; set; }

        [IndexField("body")]
        public string Body { get; set; }

        public string Category { get; set; }
        public string CategoryUrl { get; set; }
    }
}
