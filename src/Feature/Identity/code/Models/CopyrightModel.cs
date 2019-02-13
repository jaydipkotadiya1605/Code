namespace Sitecore.Feature.Identity.Models
{
    using System.Web;
    using System.Collections.Generic;

    public class CopyrightModel
    {
        public HtmlString Logo { get; set; }
        public HtmlString Text { get; set; }
        public IEnumerable<HtmlString> Links { get; set; }
    }
}