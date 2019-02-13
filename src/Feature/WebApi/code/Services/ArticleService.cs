using Sitecore.Feature.WebApi.Models;
using Sitecore.Foundation.DependencyInjection;
using Sitecore.Foundation.Search.Repositories;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Foundation.Search.Models.Index;
using Sitecore.ContentSearch.Linq.Utilities;
using FraserContent = Sitecore.Foundation.FrasersContent;
using Sitecore.ContentSearch.Utilities;
using System;

namespace Sitecore.Feature.WebApi.Services
{
    [Service(typeof(IArticleService))]
    public class ArticleService : IArticleService
    {
        private IArticleSearchRepository ArticleSearchRepository { get; }
        private ISiteSearchRepository SiteSearchRepository { get; }
        public ArticleService(IArticleSearchRepository articleSearchRepository, ISiteSearchRepository siteSearchRepository)
        {
            this.ArticleSearchRepository = articleSearchRepository;
            this.SiteSearchRepository = siteSearchRepository;
        }

        public List<ArticleInfo> GetArticles(string siteCode, string category, int pageNo, int pageSize)
        {
            if (string.IsNullOrEmpty(category))
                return new List<ArticleInfo>();

            Data.ID mallId = null;
            if (!string.IsNullOrEmpty(siteCode))
            {
                var siteItems = SiteSearchRepository.GetSitebySiteCode(siteCode);
                if (siteItems.TotalNumberOfResults == 1)
                {
                    mallId = siteItems.ScoredItems.First().Item.ID;
                }
                else
                    return new List<ArticleInfo>();
            }
            else
            {
                mallId = FraserContent.Templates.FrasersRewards.ID;
            }

            var results = this.ArticleSearchRepository.GetArticleItems(category, mallId, pageNo, pageSize);
            return results.ScoredItems.Select(r => new ArticleInfo(r.Item))?.ToList();
        }
    }
}