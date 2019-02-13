using Sitecore.Foundation.Search.Models;
using System.Collections.Generic;

namespace Sitecore.Feature.Article.Models
{
    public class ArticleItems : PageSettingExtend
    {
        public List<ArticleItem> Items { get; set; }
    }
}