using Sitecore.Feature.Blog.Repositories;
using FrasersContent = Sitecore.Foundation.FrasersContent;
using System.Web.Mvc;
using Sitecore.Mvc.Presentation;

namespace Sitecore.Feature.Blog.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogRepository _blogRepository;

        public BlogController(IBlogRepository blogRepository)
        {
            this._blogRepository = blogRepository;
        }

        public ActionResult BlogList(string category = "", int pageIndex = 0)
        {
            category = Request.QueryString[FrasersContent.Constants.QueryString.Category] != null 
                    ? Request.QueryString[FrasersContent.Constants.QueryString.Category].ToString() : category;

            var pageValue = Request.QueryString[FrasersContent.Constants.QueryString.PageIndex] ?? FrasersContent.Constants.StrPageIndexDefault;
            pageIndex = int.TryParse(pageValue, out pageIndex) ? int.Parse(pageValue): FrasersContent.Constants.PageIndexDefault;

            var strPageSize = RenderingContext.Current.Rendering.Parameters[FrasersContent.Constants.PaggingParameters.PageSize];
            int pageSize = 0;
            pageSize = int.TryParse(strPageSize, out pageSize)
                                ? int.Parse(strPageSize) : FrasersContent.Constants.DedaultPageSize;
            var totalItems = pageSize * pageIndex;

            var items = _blogRepository.GetBlogs(category, FrasersContent.Constants.DedaultPage, totalItems);
            items.PageIndex = pageIndex;
            return View("BlogList", items);
        }

        public ActionResult RelatedBlog()
        {
            var items = _blogRepository.GetRelatedBlogs(RenderingContext.Current.Rendering);
            return View("RelatedBlog", items);
        }
    }
}