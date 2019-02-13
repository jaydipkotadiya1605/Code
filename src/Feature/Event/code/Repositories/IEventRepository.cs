using Sitecore.Data.Items;
using Sitecore.Feature.Event.Models;

namespace Sitecore.Feature.Event.Repositories
{
    public interface IEventRepository
    {
        EventItems GetEvents(string category, string mallId, int page = 0, int pageSize = 100);
        EventAndArticleItems GetEventsAndArticles(string category, string mallId, int page = 0, int pageSize = 100);
        string GetLinkListPage(string itemID);
        string GetMallName(Item item);
    }
}
