namespace Sitecore.Foundation.Workflow.Steps
{
    using System;
    using Sitecore.Data;
    using Sitecore.Web.UI.Sheer;
    using Sitecore.Workflows.Simple;
    using Sitecore.Foundation.Workflow;
    using Sitecore.Foundation.SitecoreExtensions.Extensions;
    using Sitecore.Data.Items;
    using Sitecore.Data.Fields;
    using Sitecore.StringExtensions;

    public class MallAdminApproveMallContent : BaseWorkflowStep
    {
        public void Process(WorkflowPipelineArgs eventArgs)
        {
            var currentItem = eventArgs.DataItem;
            if (!currentItem.Fields[Multisite.Templates.MainSite.Fields.IsDisplayOnMainSite].IsChecked())
            {
                this.DeleteItemIfExisted(currentItem);
            }
            else
            {
                this.CopyItemToMainSite(eventArgs);
            }
            
        }
        
        private void DeleteItemIfExisted(Item currentItem)
        {
            using (new SecurityModel.SecurityDisabler())
            {
                var copiedItem = this.GetCopiedItemFromMallToMain(currentItem);
                if (copiedItem != null)
                {
                    ClientPipelineArgs cpa = new ClientPipelineArgs();
                    cpa.Parameters.Add("copiedItemId", copiedItem.ID.ToString());
                    cpa.Parameters.Add("msg", "Are you sure to delete the item in main site?");
                    Context.ClientPage.Start(this, "DeleteDialogProcessor", cpa);
                }
            }
        }
        private Item GetCopiedItemFromMallToMain(Item currentItem)
        {
            var sourceId = (ReferenceField)currentItem.Fields[HiddenFields.Templates.HiddenField.Fields.SourceId];
            if (!string.IsNullOrEmpty(sourceId.Value)) return null; // Item come from main site
            NameValueListField targetIds = currentItem.Fields[HiddenFields.Templates.HiddenField.Fields.TargetIds];
            if (targetIds.Value.IsNullOrEmpty()) return null; // Item is sent to main site
            var copiedItem = this.mallSiteWorkflowRepository.GetCopiedItemInMainSite(currentItem);
            return copiedItem;
        }
        
        #region Dialog
        protected void DeleteDialogProcessor(ClientPipelineArgs args)
        {
            var copiedItemId = args.Parameters["copiedItemId"];
            var msg = args.Parameters["msg"];
            if (!args.IsPostBack)
            {
                SheerResponse.Confirm(msg);
                args.WaitForPostBack(true);
            }
            else
            {
                // The result of a dialog is handled because a post back has occurred
                switch (args.Result)
                {
                    case "yes":
                        try
                        {
                            using (new SecurityModel.SecurityDisabler())
                            {
                                var copiedItem = Context.ContentDatabase.GetItem(new ID(copiedItemId));
                                if (copiedItem == null) return;
                                copiedItem.Delete();
                            }
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine(exception);
                            throw;
                        }
                        break;

                    case "no":
                        break;
                }
            }
        }
        #endregion

    }
}