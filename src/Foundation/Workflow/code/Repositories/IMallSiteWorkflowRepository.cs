namespace Sitecore.Foundation.Workflow.Repositories
{
    using System;
    using Sitecore.Data;
    using Sitecore.Data.Items;

    public interface IMallSiteWorkflowRepository
    {
        Item GetCopiedItemInMainSite(Item sourceItem);
        Item TryGetSourceItemBy(Item copiedItem);
        void CopyItemToMainSite(Item sourceItem, ID newState);
        void ReplaceItemOnMainSite(Item sourceItem, Item removeItem, ID newState);
    }
}
