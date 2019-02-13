namespace Sitecore.Foundation.Workflow.Repositories
{
    using Sitecore.Buckets.Managers;
    using Sitecore.Data;
    using Sitecore.Data.Fields;
    using Sitecore.Data.Items;
    using Sitecore.Exceptions;
    using Sitecore.Foundation.DependencyInjection;
    using Sitecore.Foundation.Multisite;
    using Sitecore.Foundation.Multisite.Extensions;
    using Sitecore.StringExtensions;
    using System;
    using System.Collections.Specialized;

    [Service(typeof(IMallSiteWorkflowRepository))]
    public class MallSiteWorkflowRepository : IMallSiteWorkflowRepository
    {
        public Item GetCopiedItemInMainSite(Item sourceItem)
        {
            var targetField = (NameValueListField)sourceItem.Fields[HiddenFields.Templates.HiddenField.Fields.TargetIds];
            if (targetField.Value.IsNullOrEmpty()) return null;
            var copiedItemId = targetField.NameValues[0];
            var copyIdItem = sourceItem.Database?.GetItem(new ID(copiedItemId));
            if (copyIdItem == null) throw new ItemNotFoundException("Copied Item in MainSite");
            return copyIdItem;
        }
        public Item TryGetSourceItemBy(Item copiedItem)
        {
            var sourceField = (NameValueListField)copiedItem.Fields[HiddenFields.Templates.HiddenField.Fields.SourceId];
            return sourceField.Value.IsNullOrEmpty() 
                ? null 
                : copiedItem.Database?.GetItem(new ID(sourceField.Value));
        }
        public void CopyItemToMainSite(Item sourceItem, ID newState)
        {
            var mallSite = sourceItem.GetSiteItem();
            var mainSiteSetting = (ReferenceField)mallSite.Fields[Templates.MallSiteSetting.Fields.MainSite];
            var mainSite = mainSiteSetting.TargetItem;
            if (mainSite == null)
            {
                throw new ClientAlertException("Main Site Setting is not correct");
            }
            var destination = sourceItem.GetDestinationItem(mainSite);
            if (destination == null)
            {
                throw new ClientAlertException("Parent Item on Main Site is not exists.");
            }
            this.CopyItem(sourceItem, destination, newState);
        }

        public void ReplaceItemOnMainSite(Item sourceItem, Item removeItem, ID newState)
        {
            removeItem.Delete();
            this.CopyItemToMainSite(sourceItem, newState);
        }
        
        private void CopyItem(Item sourceItem, Item destination, ID newState)
        {
            var resultItem = BucketManager.IsItemContainedWithinBucket(sourceItem) 
                ? BucketManager.CopyItem(destination, sourceItem, true)
                : sourceItem.CopyTo(destination, sourceItem.Name);
            //Update info sourceItem
            sourceItem.Editing.BeginEdit();
            ((NameValueListField)sourceItem.Fields[HiddenFields.Templates.HiddenField.Fields.TargetIds]).NameValues
                = new NameValueCollection {{resultItem.GetSiteItem().ID.ToShortID().ToString(), resultItem.ID.Guid.ToString().ToUpper()}};
            ((CheckboxField)sourceItem.Fields[Multisite.Templates.MainSite.Fields.IsDisplayOnMainSite]).Checked = true;
            sourceItem.Editing.EndEdit();
            //Update info copiedItem
            resultItem.Editing.BeginEdit();
            resultItem.Fields[HiddenFields.Templates.HiddenField.Fields.SourceId].SetValue(sourceItem.ID.ToString(), true);
            ((MultilistField)resultItem.Fields[Templates.MallSite.Fields.SiteDisplaySettings]).Add(sourceItem.GetSiteItem().ID.ToString());
            resultItem.Fields[FieldIDs.State].Value = newState.ToString();
            resultItem.Editing.EndEdit();
        }
    }
}
