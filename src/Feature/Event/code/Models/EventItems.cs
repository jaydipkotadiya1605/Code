using Sitecore.Foundation.Search.Models;
using System.Collections.Generic;

namespace Sitecore.Feature.Event.Models
{
    public class EventItems : PageSettingExtend
    {
        public List<EventItem> Items { get; set; }
    }
}