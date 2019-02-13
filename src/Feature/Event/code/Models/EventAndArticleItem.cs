﻿namespace Sitecore.Feature.Event.Models
{
    using Sitecore.Data.Items;
    using System;

    public class EventAndArticleItem
    {
        public Item Item { get; set; }
        public string Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Alt { get; set; }
        public string Mall { get; set; }
    }
}