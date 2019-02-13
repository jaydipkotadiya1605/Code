using Sitecore.Feature.Article.Repositories;
using Sitecore.Mvc.Presentation;
using System.Web.Mvc;
using FrasersContent = Sitecore.Foundation.FrasersContent;

namespace Sitecore.Feature.Article.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleRepository _articleRepository;

        public ArticleController(IArticleRepository articleRepository)
        {
            this._articleRepository = articleRepository;
        }

        public ActionResult ArticleList(string category = "", int pageIndex = 0)
        {
            category = Request.QueryString[FrasersContent.Constants.QueryString.Category] != null 
                        ? Request.QueryString[FrasersContent.Constants.QueryString.Category].ToString() : category;

            var pageValue = Request.QueryString[FrasersContent.Constants.QueryString.PageIndex] != null 
                        ? Request.QueryString[FrasersContent.Constants.QueryString.PageIndex] : FrasersContent.Constants.StrPageIndexDefault;
            pageIndex = int.TryParse(pageValue, out pageIndex) ? int.Parse(pageValue): FrasersContent.Constants.PageIndexDefault;

            var strPageSize = RenderingContext.Current.Rendering.Parameters[FrasersContent.Constants.PaggingParameters.PageSize];
            int pageSize = 0;
            pageSize = int.TryParse(strPageSize, out pageSize)
                                ? int.Parse(strPageSize) : FrasersContent.Constants.DedaultPageSize;
            var totalItems = pageSize * pageIndex;

            var items = _articleRepository.GetArticles(category, FrasersContent.Constants.DedaultPage, totalItems);
            items.PageIndex = pageIndex;
            return View("ArticleList", items);
        }

        public ActionResult ArticleWidget()
        {
            var linkOfListPageID = RenderingContext.Current.Rendering.Parameters[FrasersContent.Constants.WidgetSettingParameters.LinkOfListPage];
            var strNumberOfScores = RenderingContext.Current.Rendering.Parameters[FrasersContent.Constants.WidgetSettingParameters.NumberOfScores];
            var strIsInStorePage = RenderingContext.Current.Rendering.Parameters[FrasersContent.Constants.WidgetSettingParameters.IsStorePage];
            var categories = RenderingContext.Current.Rendering.Parameters[FrasersContent.Constants.WidgetSettingParameters.Categories];

            int numberOfScores = 0;
            numberOfScores = int.TryParse(strNumberOfScores, out numberOfScores)
                                ? int.Parse(strNumberOfScores) : FrasersContent.Constants.DedaultWidgetScores;
            var items = _articleRepository.GetMostWantedDeals(linkOfListPageID, strIsInStorePage, categories, FrasersContent.Constants.DedaultPage, numberOfScores);
            return View("ArticleWidget", items);
        }
    }
}