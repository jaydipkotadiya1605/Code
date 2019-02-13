namespace Sitecore.Foundation.Workflow.Steps
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.Data.Fields;
    using Sitecore.Data.Items;
    using Sitecore.DependencyInjection;
    using Sitecore.Foundation.Workflow.Repositories;
    using Sitecore.StringExtensions;
    using Sitecore.Workflows.Simple;

    public class ApproveMainContent
    {
        private readonly IMainSiteWorkflowRepository mainSiteWorkflow;

        public ApproveMainContent()
        {
            this.mainSiteWorkflow = ServiceLocator.ServiceProvider.GetService<IMainSiteWorkflowRepository>();
        }
        public void Process(WorkflowPipelineArgs args)
        {
            var itemData = args.DataItem;
            if (itemData == null) return;
            var sourceItemField = (ReferenceField)itemData.Fields[HiddenFields.Templates.HiddenField.Fields.SourceId];
            if (sourceItemField.Value.IsNullOrEmpty())
            {
                this.ApproveMainContentFromMainAuthor(itemData);
            }
        }

        private void ApproveMainContentFromMainAuthor(Item itemData)
        {
            try
            {
                using (new SecurityModel.SecurityDisabler())
                {
                    this.mainSiteWorkflow.ChangeStateItemsInMalls(itemData, Constants.States.Approved);
                }
            }
            catch (Exception exception)
            {
                Context.ClientPage.ClientResponse.Alert(exception.Message);
            }
        }
    }
}