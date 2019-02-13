namespace Sitecore.Foundation.Workflow.Repositories
{
    using Sitecore.Data;
    using Sitecore.Data.Fields;
    using Sitecore.Data.Items;
    using Sitecore.Diagnostics;
    using Sitecore.Foundation.DependencyInjection;
    using System;

    [Service(typeof(IStateRepository))]
    public class StateRepository : IStateRepository
    {
        public void ChangeItemStateTo(Item item, Guid state) => this.ChangeItemState(item, new ID(state));
        public void ChangeItemStateTo(Item item, ID state) => this.ChangeItemState(item, state);
        public void RemoveItemByState(Item item, ID state)
        {
            if (item.Fields["__Workflow state"].Value.Equals(state.ToString()))
            {
                item.Delete();
            }
        }

        public ID GetStateId(Item item)
        {
            ReferenceField state = item.Fields["__Workflow state"];
            return state?.TargetItem?.ID;
        }

        private void ChangeItemState(Item item, ID state)
        {
            try
            {
                item.Editing.BeginEdit();
                try
                {
                    item.Fields["__Workflow state"].Value = state.ToString();
                }
                catch (Exception exception)
                {
                    Log.Error(exception.Message, exception, typeof(IMallSiteWorkflowRepository));
                    throw;
                }
                item.Editing.EndEdit();
            }
            catch (Exception exception)
            {
                Log.Error(exception.Message, exception, typeof(IMallSiteWorkflowRepository));
                throw;
            }
        }
    }
}