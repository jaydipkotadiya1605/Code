namespace Sitecore.Foundation.Workflow.Repositories
{
    using Sitecore.Data;
    using Sitecore.Data.Items;

    public interface IMainSiteWorkflowRepository
    {
        void CloneOrRemoveItemsInMalls(Item sourceItem);
        void ChangeStateItemsInMalls(Item sourceItem, ID state);
    }
}
