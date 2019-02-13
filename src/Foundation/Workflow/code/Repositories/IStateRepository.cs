namespace Sitecore.Foundation.Workflow.Repositories
{
    using System;
    using Sitecore.Data;
    using Sitecore.Data.Items;

    public interface IStateRepository
    {
        void ChangeItemStateTo(Item item, Guid state);
        void ChangeItemStateTo(Item item, ID state);
        void RemoveItemByState(Item item, ID state);
        ID GetStateId(Item item);
    }
}