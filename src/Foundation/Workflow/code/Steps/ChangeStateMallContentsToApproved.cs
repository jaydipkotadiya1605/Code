namespace Sitecore.Foundation.Workflow.Steps
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.Data;
    using Sitecore.DependencyInjection;
    using Sitecore.Foundation.Workflow.Repositories;
    using Sitecore.Web.UI.Sheer;
    using Sitecore.Workflows.Simple;

    public class ChangeStateMallContentsToApproved
    {
        private readonly IMainSiteWorkflowRepository mainSiteWorkflow;

        public ChangeStateMallContentsToApproved()
        {
            this.mainSiteWorkflow = ServiceLocator.ServiceProvider.GetService<IMainSiteWorkflowRepository>();
        }
        public void Process(WorkflowPipelineArgs args)
        {
            var itemData = args.DataItem;
            if (itemData == null) return;
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