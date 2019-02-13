namespace Sitecore.Feature.Event.Repositories
{
    using Sitecore.Data.Items;
    using System.Linq;
    using Sitecore.Feature.Event.Models;
    using Sitecore.Foundation.SitecoreExtensions.Extensions;
    using Sitecore.Foundation.DependencyInjection;
    using FrasersContent = Foundation.FrasersContent;
    using Sitecore.Foundation.Search.Repositories;
    using Sitecore.ContentSearch.Utilities;
    using Sitecore.Foundation.Abstractions.SitecoreContext;
    using Sitecore.Foundation.Dictionary.Repositories;
    using System.Collections.Generic;

    [Service(typeof(IEventRepository))]
    public class EventRepository : IEventRepository
    {
        public IEventSearchRepository EventSearchRepository { get; }
        private readonly ISitecoreContext _sitecoreContext;
        public EventRepository(ISitecoreContext sitecoreContext,
            IEventSearchRepository eventSearchRepository)
        {
            this._sitecoreContext = sitecoreContext;
            this.EventSearchRepository = eventSearchRepository;
        }

        public EventItems GetEvents(string category, string mallId, int page = 0, int pageSize = 100)
        {
            var path = _sitecoreContext.Site.RootPath;

            var result = EventSearchRepository.GetEventItems(category, mallId, page, pageSize, path);
            var hasMoreResult = pageSize < result.TotalNumberOfResults;

            return new EventItems() {
                Items = result.ScoredItems.Select(x => CreateEventItem(x.Item)).Where(x => x!= null).ToList(),
                HasMoreResult = hasMoreResult,
                PageSize = pageSize
            };
        }

        public EventAndArticleItems GetEventsAndArticles(string category, string mallId, int page = 0, int pageSize = 100)
        {
            var path = _sitecoreContext.Site.RootPath;

            var result = EventSearchRepository.GetEventItems(category, mallId, page, pageSize, path);
            var hasMoreResult = pageSize < result.TotalNumberOfResults;

            return new EventAndArticleItems()
            {
                Items = result.ScoredItems.Select(x => CreateEventAndArticleItem(x.Item)).Where(x => x != null).ToList(),
                HasMoreResult = hasMoreResult,
                PageSize = pageSize
            };
        }

        public string GetLinkListPage(string itemID)
        {
            if (itemID != null)
            {
                var item = _sitecoreContext.Database.GetItem(new Data.ID(itemID));
                if (item != null)
                {
                    return item.IsDerived(FrasersContent.Templates.Link.ID) ? item.LinkFieldUrl(FrasersContent.Templates.Link.Fields.Link) : item.Url();
                }
            }
            return FrasersContent.Constants.DefaultLink;
        }

        public string GetMallName(Item item)
        {
            var mallName = string.Empty;
            if (item != null && item.IsDerived(FrasersContent.Templates.Event.ID))
            {
                var mallIDs = item.GetMultiListValueItems(FrasersContent.Templates.MallSite.Fields.DisplayOnMalls)
                             .Select(x => x.ID).ToList();
                if (mallIDs.Count == 1)
                {
                    mallName = _sitecoreContext.Database.GetItem(mallIDs[0]).GetString(FrasersContent.Templates.Identity.Fields.SiteName);
                }
                else if (mallIDs.Count > 1)
                {
                    mallName = DictionaryPhraseRepository.Current.Get(FrasersContent.Constants.MultipleMalls, FrasersContent.Constants.MultipleMallsText);
                }
            }

            return mallName;
        }

        private EventItem CreateEventItem(Item item)
        {
            if (item != null && item.IsDerived(FrasersContent.Templates.Event.ID))
            {
                var thumbnailField = ((Data.Fields.ImageField)item.Fields[FrasersContent.Templates.Event.Fields.Thumbnail]);
                string altText = thumbnailField.Alt;

                return new EventItem
                {
                    Item = item,
                    Id = IdHelper.NormalizeGuid(item.ID),
                    Title = item.Fields[FrasersContent.Templates.Event.Fields.Title].Value,
                    EndDate = DateUtil.ToServerTime(item.GetDateTime(FrasersContent.Templates.Event.Fields.EndDate)),
                    StartDate = DateUtil.ToServerTime(item.GetDateTime(FrasersContent.Templates.Event.Fields.StartDate)),
                    Url = item.Url(),
                    Alt = string.IsNullOrEmpty(altText) ? item.Fields[FrasersContent.Templates.Event.Fields.Title].Value : altText,
                    Mall = GetMallName(item)
                };
              
            }
           
            return null;
        }

        private EventAndArticleItem CreateEventAndArticleItem(Item item)
        {
            if (item != null && item.IsDerived(FrasersContent.Templates.Event.ID))
            {
                var thumbnailField = ((Data.Fields.ImageField)item.Fields[FrasersContent.Templates.Event.Fields.Thumbnail]);
                string altText = thumbnailField.Alt;

                return new EventAndArticleItem
                {
                    Item = item,
                    Id = IdHelper.NormalizeGuid(item.ID),
                    Title = item.Fields[FrasersContent.Templates.Event.Fields.Title].Value,
                    EndDate = DateUtil.ToServerTime(item.GetDateTime(FrasersContent.Templates.Event.Fields.EndDate)),
                    StartDate = DateUtil.ToServerTime(item.GetDateTime(FrasersContent.Templates.Event.Fields.StartDate)),
                    Url = item.Url(),
                    Alt = string.IsNullOrEmpty(altText) ? item.Fields[FrasersContent.Templates.Event.Fields.Title].Value : altText,
                    Mall = GetMallName(item)
                };

            }

            return null;
        }
    }
}