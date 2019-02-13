using Sitecore.Feature.WebApi.Models;
using System.Collections.Generic;

namespace Sitecore.Feature.WebApi.Services
{
    public interface IArticleService
    {
        List<ArticleInfo> GetArticles(string siteCode, string category, int pageNo, int pageSize);
    }
}