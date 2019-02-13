using System;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.Data;
using Sitecore.DependencyInjection;
using Sitecore.Foundation.Workflow.Repositories;
using Sitecore.Web.UI.Sheer;
using Sitecore.Workflows.Simple;
using Sitecore.Data.Items;

namespace Sitecore.Foundation.Workflow.Steps
{
    public class BaseWorkflowStep
    {
        protected readonly IMallSiteWorkflowRepository mallSiteWorkflowRepository;
        protected readonly IStateRepository stateRepository;
        public BaseWorkflowStep()
        {
            mallSiteWorkflowRepository = ServiceLocator.ServiceProvider.GetService<IMallSiteWorkflowRepository>();
            stateRepository = ServiceLocator.ServiceProvider.GetService<IStateRepository>();
        }
        protected void CopyItemToMainSite(WorkflowPipelineArgs eventArgs)
        {
            var currentItem = eventArgs.DataItem;
            if (currentItem.Fields[HiddenFields.Templates.HiddenField.Fields.SourceId].HasValue)
            {
                Context.ClientPage.ClientResponse.Alert("This action can't be process. The content is belong to main site.");
                eventArgs.AbortPipeline();
                return;
            }
            try
            {
                using (new SecurityModel.SecurityDisabler())
                {
                    var copiedItem = this.mallSiteWorkflowRepository.GetCopiedItemInMainSite(currentItem);
                    if (copiedItem == null)
                    {
                        this.mallSiteWorkflowRepository.CopyItemToMainSite(currentItem, Constants.States.WaitingForMainSiteApproval);
                    }
                    else
                    {
                        var state = this.stateRepository.GetStateId(copiedItem);
                        ClientPipelineArgs cpa = new ClientPipelineArgs();
                        cpa.Parameters.Add("currentItemId", currentItem.ID.ToString());
                        cpa.Parameters.Add("copiedItemId", copiedItem.ID.ToString());
                        if (state.Equals(Constants.States.Draft) || state.Equals(Constants.States.WaitingForMainSiteApproval))
                        {
                            cpa.Parameters.Add("msg", "This item is already exists on main site. Do you want to replace?");
                            Context.ClientPage.Start(this, "CopyItemToMainSiteDialog", cpa);
                        }
                        if (state.Equals(Constants.States.Approved))
                        {
                            cpa.Parameters.Add("msg", "The item is approved by the main admin. Do you want to replace and turn back it into 'Waiting for approve'?");
                            Context.ClientPage.Start(this, "CopyItemToMainSiteDialog", cpa);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Context.ClientPage.ClientResponse.Alert(exception.Message);
            }
        }
         
        #region Sitecore Dialog Messages 
        public void CopyItemToMainSiteDialog(ClientPipelineArgs args)
        {
            var currentItemId = args.Parameters["currentItemId"];
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
                                var currentItem = Context.ContentDatabase.GetItem(new ID(currentItemId));
                                var copiedItem = Context.ContentDatabase.GetItem(new ID(copiedItemId));
                                this.mallSiteWorkflowRepository.ReplaceItemOnMainSite(currentItem, copiedItem, Constants.States.WaitingForMainSiteApproval);
                            }
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine(exception);
                            throw;
                        }
                        Context.ClientPage.ID = currentItemId;
                        break;

                    case "no":
                        break;
                }
            }
        }
        #endregion
    }
}