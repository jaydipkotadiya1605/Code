using Sitecore.Feature.Article.Models;

namespace Sitecore.Feature.Article.Repositories
{
    public interface IArticleRepository
    {
        ArticleItems GetArticles(string category, int page = 0, int pageSize = 100);
        ArticleWidgetItems GetMostWantedDeals(string listPageItemId, string strIsInStorePage, string categories, int page = 0, int pageSize = 100);
    }
}
