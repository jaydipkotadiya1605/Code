using Sitecore.Foundation.Search.Models;
using System.Collections.Generic;

namespace Sitecore.Feature.Blog.Models
{
    public class BlogItems : PageSettingExtend
    {
        public List<BlogItem> Items { get; set; }
    }
}