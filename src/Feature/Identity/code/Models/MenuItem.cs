using System.Collections.Generic;
using System.Web;

namespace Sitecore.Feature.Identity.Models
{
    public class MenuItem
    {
        public HtmlString Url { get; set; }
    }

    public class MenuItems
    {
        public IList<MenuItem> Items { get; set; }
    }
}