namespace Sitecore.Foundation.Abstractions.Services
{
    using Data;
    using Data.Items;
    using System;
    using DependencyInjection;
    using SitecoreContext;

    [Service(typeof(ISitecoreDataService))]
    public class SitecoreDataService : ISitecoreDataService
    {
        private readonly ISitecoreContext sitecoreContext;
        
        public SitecoreDataService(ISitecoreContext context)
        {
            this.sitecoreContext = context;
        }
        
        public Item GetItem(ID id)
        {
            if (this.sitecoreContext != null && this.sitecoreContext.Database != null)
            {
                return this.sitecoreContext.Database.GetItem(id);
            }
            return null;
        }

        public Item GetItem(Guid id)
        {
            ID sitecoreId = new ID(id);
            return this.GetItem(sitecoreId);
        }

        public Item GetItem(string path)
        {
            if (this.sitecoreContext != null && this.sitecoreContext.Database != null)
            {
                return this.sitecoreContext.Database.GetItem(path);
            }
            return null;
        }
    }
}