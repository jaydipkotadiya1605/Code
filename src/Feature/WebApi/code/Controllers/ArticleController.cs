using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Feature.WebApi.Formatters;
using Sitecore.Feature.WebApi.Models;
using Sitecore.Feature.WebApi.Services;
using Sitecore.Foundation.ItemResolver.Extensions;
using Sitecore.Foundation.Search.Repositories;
using System;
using System.Web.Http;

namespace Sitecore.Feature.WebApi.Controllers
{
    [RoutePrefix(Constants.ApiRouting.Root)]
    public class ArticleController : BaseApiController
    {
        private IArticleService ArticleService { get; }
        public ArticleController(IArticleService articleService) {
            this.ArticleService = articleService;
        }

        [Route(Constants.ApiRouting.ArticleListForMall)]
        public IHttpActionResult GetArticles(string mall, string category)
        {
            try
            {
                this.GetPagingInfo(ref this.pageNo, ref this.pageSize);
                var output = new ArticleInfoListOutput(Constants.ApiStatus.Success, this.ArticleService.GetArticles(mall, category, this.pageNo, this.pageSize));
                return this.JsonResult<ArticleInfoListOutput>(output);
            }
            catch (Exception ex)
            {
                Log.Error($":GetArticles({mall}, {category}). Error message: {ex.Message}", ex, this);
                var error = new JsonOutput(Constants.ApiStatus.Fail, ex.Message);
                return this.JsonResult<JsonOutput>(error);
            }
        }

        [Route(Constants.ApiRouting.ArticleList)]
        public IHttpActionResult GetArticles(string category)
        {
            try
            {
                this.GetPagingInfo(ref this.pageNo, ref this.pageSize);
                var output = new ArticleInfoListOutput(Constants.ApiStatus.Success, this.ArticleService.GetArticles(string.Empty, category, this.pageNo, this.pageSize));
                return this.JsonResult<ArticleInfoListOutput>(output);
            }
            catch (Exception ex)
            {
                Log.Error($":GetArticles({category}). Error message: {ex.Message}", ex, this);
                var error = new JsonOutput(Constants.ApiStatus.Fail, ex.Message);
                return this.JsonResult<JsonOutput>(error);
            }
        }

        [Route(Constants.ApiRouting.ArticleDetail)]
        public IHttpActionResult GetArticle(string id)
        {
            try
            {
                var item = Context.Database.GetItem(ID.Parse(id));
                if (!item.IsOnCurrentSite())
                {
                    throw new ArgumentException($"Article {id} is not found");
                }

                var output = new ArticleOutput(Constants.ApiStatus.Success, new Article(item));
                return this.JsonResult<ArticleOutput>(output);
            }
            catch (Exception ex)
            {
                Log.Error($":GetArticle({id}). Error message: {ex.Message}", ex, this);
                var error = new JsonOutput(Constants.ApiStatus.Fail, ex.Message);
                return this.JsonResult<JsonOutput>(error);
            }
        }
    }
}