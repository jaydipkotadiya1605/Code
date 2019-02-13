namespace Sitecore.Foundation.Search.Repositories
{
    using Sitecore.ContentSearch;
    using Sitecore.ContentSearch.Linq;
    using Sitecore.Foundation.Abstractions.SitecoreContext;
    using Sitecore.Foundation.DependencyInjection;
    using Sitecore.Foundation.Search.Models;
    using Sitecore.Foundation.Search.Models.Index;
    using System.Linq;
    using Sitecore.ContentSearch.Linq.Utilities;
    using Sitecore.ContentSearch.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    [Service(typeof(IEventAndArticleRepository))]
    public class EventAndArticleRepository : SearchRepository, IEventAndArticleRepository
    {
        public EventAndArticleRepository(ISitecoreContext sitecoreContext) : base(sitecoreContext)
        {

        }

        public SearchItems GetArticleItems(List<string> categoryNames, string storeId = "", int page = 0, int pageSize = 100, string path = "")
        {

            return null;
        }

        public SearchItems GetEventItems(string categoryName, string mall, int page = 0, int pageSize = 100, string path = "")
        {

            return null;
        }

        public SearchItems GetEventAndArticleItems(string categoryName, string mall, int page = 0, int pageSize = 100, string path = "")
        {

            return null;
        }

    }
}