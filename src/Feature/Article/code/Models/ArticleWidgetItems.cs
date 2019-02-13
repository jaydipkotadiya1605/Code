using Sitecore.Foundation.Search.Models;
using System.Collections.Generic;

namespace Sitecore.Feature.Article.Models
{
    public class ArticleWidgetItems : PageSettingExtend
    {
       public List<ArticleWidgetItem> Items { get; set; }
       public string LinkOfListPage { get; set; }
    }
}