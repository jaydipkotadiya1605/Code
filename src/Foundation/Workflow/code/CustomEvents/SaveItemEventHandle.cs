namespace Sitecore.Foundation.Workflow.CustomEvents
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.Data;
    using Sitecore.Data.Fields;
    using Sitecore.Data.Items;
    using Sitecore.DependencyInjection;
    using Sitecore.Events;
    using Sitecore.Foundation.Multisite;
    using Sitecore.Foundation.Multisite.Extensions;
    using Sitecore.Foundation.SitecoreExtensions.Extensions;
    using Sitecore.Foundation.Workflow.Repositories;
    using Sitecore.StringExtensions;

    public class SaveItemEventHandle
    {
        private readonly IMainSiteWorkflowRepository mainSiteWorkflow;
        public SaveItemEventHandle()
        {
            this.mainSiteWorkflow = ServiceLocator.ServiceProvider.GetService<IMainSiteWorkflowRepository>();
        }
        public void OnItemSaving(object sender, EventArgs args)
        {
            var savingItem = Event.ExtractParameter(args, 0) as Item;
            var eventArgs = args as SitecoreEventArgs;
            if (savingItem == null || eventArgs == null) return;
            //Copy Item from Main to Mall
            if (savingItem.IsDerived(Templates.MallSite.ID) && savingItem.IsBelongToMainSite())
            {
                try
                {
                    using (new SecurityModel.SecurityDisabler())
                    {
                        CopyItemFromMainToMall(savingItem);
                    }
                }
                catch (Exception exception)
                {
                    Context.ClientPage.ClientResponse.Alert(exception.Message);
                    eventArgs.Result.Cancel = true;
                }
                
            }
        }
        private void CopyItemFromMainToMall(Item savingItem)
        {
            var sourceId = (ReferenceField)savingItem.Fields[HiddenFields.Templates.HiddenField.Fields.SourceId];
            MultilistField mallSitesInput = savingItem.Fields[Templates.MallSite.Fields.SiteDisplaySettings];
            NameValueListField targetIds = savingItem.Fields[HiddenFields.Templates.HiddenField.Fields.TargetIds];
            if (mallSitesInput.Value.IsNullOrEmpty() && targetIds.Value.IsNullOrEmpty()) return; // mall sites is not input
            if (!string.IsNullOrEmpty(sourceId.Value)) return; // Item come from mall site
            this.mainSiteWorkflow.CloneOrRemoveItemsInMalls(savingItem);
        }
    }
}