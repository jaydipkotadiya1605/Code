using System;
namespace Sitecore.Feature.Event.Models
{
    using Sitecore.Foundation.Search.Models;
    using System.Collections.Generic;

    public class EventAndArticleItems : PageSettingExtend
    {
        public List<EventAndArticleItem> Items { get; set; }
    }
}