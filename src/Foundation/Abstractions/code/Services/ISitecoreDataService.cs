namespace Sitecore.Foundation.Abstractions.Services
{
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using System;

    public interface ISitecoreDataService
    {
        Item GetItem(Guid id);
        Item GetItem(ID id);
        Item GetItem(string path);
    }
}
