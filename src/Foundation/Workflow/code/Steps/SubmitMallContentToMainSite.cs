namespace Sitecore.Foundation.Workflow.Steps
{
    using System;
    using System.Web.UI.WebControls;
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.Data;
    using Sitecore.Data.Fields;
    using Sitecore.DependencyInjection;
    using Sitecore.Foundation.Workflow.Repositories;
    using Sitecore.Foundation.HiddenFields;
    using Sitecore.Web.UI.Sheer;
    using Sitecore.Workflows.Simple;
    using Sitecore.Foundation.Workflow;

    public class SubmitMallContentToMainSite : BaseWorkflowStep
    {
        public void Process(WorkflowPipelineArgs eventArgs)
        {
            this.CopyItemToMainSite(eventArgs);
        }
    }
}