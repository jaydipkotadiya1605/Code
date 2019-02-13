using Sitecore.Feature.Event.Repositories;
using FrasersContent = Sitecore.Foundation.FrasersContent;
using Sitecore.Mvc.Presentation;
using System.Web.Mvc;
using Sitecore.Data.Items;
using System.Linq;
using Sitecore.Data.Fields;

namespace Sitecore.Feature.Event.Controllers
{
    public class EventController : Controller
    {
        private readonly IEventRepository _eventRepository;

        public EventController(IEventRepository eventRepository)
        {
            this._eventRepository = eventRepository;
        }

        public ActionResult EventList(string category = "", string mallId = "", int pageIndex = 0)
        {
            category = Request.QueryString[FrasersContent.Constants.QueryString.Category] != null
                    ? Request.QueryString[FrasersContent.Constants.QueryString.Category].ToString() : category;
            mallId = Request.QueryString[FrasersContent.Constants.QueryString.MallId] != null
                    ? Request.QueryString[FrasersContent.Constants.QueryString.MallId].ToString() : mallId;

            var pageValue = Request.QueryString[FrasersContent.Constants.QueryString.PageIndex] ?? FrasersContent.Constants.StrPageIndexDefault;
            pageIndex = int.TryParse(pageValue, out pageIndex) ? int.Parse(pageValue) : FrasersContent.Constants.PageIndexDefault;

            var strPageSize = RenderingContext.Current.Rendering.Parameters[FrasersContent.Constants.PaggingParameters.PageSize];
            int pageSize = 0;
            pageSize = int.TryParse(strPageSize, out pageSize)
                                ? int.Parse(strPageSize) : FrasersContent.Constants.DedaultPageSize;
            var totalItems = pageSize * pageIndex;
            var items = _eventRepository.GetEvents(category, mallId, FrasersContent.Constants.DedaultPage, totalItems);
            items.PageIndex = pageIndex;
            return View("EventList", items);
        }

        public ActionResult EventAndArticleList(string category = "", string mallId = "", int pageIndex = 0)
        {
            category = Request.QueryString[FrasersContent.Constants.QueryString.Category] != null
                    ? Request.QueryString[FrasersContent.Constants.QueryString.Category].ToString() : category;
            mallId = Request.QueryString[FrasersContent.Constants.QueryString.MallId] != null
                    ? Request.QueryString[FrasersContent.Constants.QueryString.MallId].ToString() : mallId;

            var pageValue = Request.QueryString[FrasersContent.Constants.QueryString.PageIndex] ?? FrasersContent.Constants.StrPageIndexDefault;
            pageIndex = int.TryParse(pageValue, out pageIndex) ? int.Parse(pageValue) : FrasersContent.Constants.PageIndexDefault;

            var strPageSize = RenderingContext.Current.Rendering.Parameters[FrasersContent.Constants.PaggingParameters.PageSize];
            var eventAndArticleTabFolder = RenderingContext.Current.Rendering.Parameters[Constants.EventAndArticleTabField];
            if (string.IsNullOrWhiteSpace(eventAndArticleTabFolder) || string.IsNullOrWhiteSpace(category))
            {
                // Get All Articles and Events
                
            }
            else
            {
                // Get Event or Article
                var tabFolderItem = !string.IsNullOrWhiteSpace(eventAndArticleTabFolder) ? Sitecore.Context.Database.GetItem(eventAndArticleTabFolder) : null;
                string eventCategory = GetEventCategory(tabFolderItem, category);
                string articleCategory = GetArticleCategory(tabFolderItem, category);
                if (!string.IsNullOrWhiteSpace(eventCategory) || !string.IsNullOrWhiteSpace(articleCategory))
                {
                    // Get combined results
                }
                else if (!string.IsNullOrWhiteSpace(eventCategory))
                {
                    // Get only Events
                }
                else if (!string.IsNullOrWhiteSpace(articleCategory))
                {
                    // Get Articles only
                }
            }
            int pageSize = 0;
            pageSize = int.TryParse(strPageSize, out pageSize)
                                ? int.Parse(strPageSize) : FrasersContent.Constants.DedaultPageSize;
            var totalItems = pageSize * pageIndex;
            //var items = _eventRepository.GetEvents(category, mallId, FrasersContent.Constants.DedaultPage, totalItems);
            //items.PageIndex = pageIndex;
            return View("EventAndArticleList", null /*items*/);
        }

        public ActionResult EventWidget()
        {
            var linkOfListPageID = RenderingContext.Current.Rendering.Parameters[FrasersContent.Constants.WidgetSettingParameters.LinkOfListPage];
            var strNumberOfScores = RenderingContext.Current.Rendering.Parameters[FrasersContent.Constants.WidgetSettingParameters.NumberOfScores];
            int numberOfScores = 0;
            numberOfScores = int.TryParse(strNumberOfScores, out numberOfScores)
                                ? int.Parse(strNumberOfScores) : FrasersContent.Constants.DedaultWidgetScores;
            ViewBag.Link = _eventRepository.GetLinkListPage(linkOfListPageID);
            var items = _eventRepository.GetEvents(string.Empty, string.Empty, FrasersContent.Constants.DedaultPage, numberOfScores);
            return View("EventWidget", items);
        }

        public ActionResult EventDetail()
        {
            var rendering = RenderingContext.Current.Rendering.Model as RenderingModel;
            ViewBag.MallName = _eventRepository.GetMallName(rendering.Item);
            return View("EventDetail", rendering);
        }

        private string GetEventCategory(Item tabFolderItem, string category)
        {
            string categoryName = string.Empty;
            if (tabFolderItem == null)
            {
                return categoryName;
            }

            var tabItem = tabFolderItem.Children.FirstOrDefault(x => x.Fields["Value"].Value == category);
            categoryName = ((ReferenceField)tabItem?.Fields["Event Category"])?.TargetItem?.Fields["Value"]?.Value;

            return categoryName;
        }

        private string GetArticleCategory(Item tabFolderItem, string category)
        {
            string categoryName = string.Empty;
            if (tabFolderItem == null)
            {
                return categoryName;
            }

            var tabItem = tabFolderItem.Children.FirstOrDefault(x => x.Fields["Value"].Value == category);
            categoryName = ((ReferenceField)tabItem?.Fields["Article Category"])?.TargetItem?.Fields["Value"]?.Value;

            return categoryName;
        }
    }
}