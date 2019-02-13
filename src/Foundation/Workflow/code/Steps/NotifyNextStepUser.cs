using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.DependencyInjection;
using Sitecore.Diagnostics;
using Sitecore.Foundation.Multisite.Extensions;
using Sitecore.Foundation.Workflow.Helpers;
using Sitecore.Foundation.Workflow.Services;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Security.Accounts;
using Sitecore.SecurityModel;
using Sitecore.Workflows.Simple;
using Sitecore.Foundation.Workflow.Repositories;

namespace Sitecore.Foundation.Workflow.Steps
{
    public class NotifyNextStepUser
    {
        private readonly IWorkflowNotifyService _workflowNotifyService;
        private readonly IMallSiteWorkflowRepository _mallSiteWorkflowRepository;

        public NotifyNextStepUser()
        {
            _workflowNotifyService = ServiceLocator.ServiceProvider.GetService<IWorkflowNotifyService>();
            _mallSiteWorkflowRepository = ServiceLocator.ServiceProvider.GetService<IMallSiteWorkflowRepository>();
        }
        public void Process(WorkflowPipelineArgs args)
        {
            Item contentItem = args.DataItem;
            string workflowComment = (!string.IsNullOrEmpty(args.CommentFields[Constants.Comments])) ? args.CommentFields[Constants.Comments] : Constants.NoComment;

            try
            {
                using (new SecurityDisabler())
                {
                    Item notifyStepItem = args.ProcessorItem.InnerItem;
                    Item currentWorkflowStep = WorkflowHelper.GetCurrentState(args);  // Current workflow state
                    Item nextWorkflowStep = WorkflowHelper.GetNextState(args); // Next workflow state

                    var from = notifyStepItem.Fields[Templates.NotifyNextStepUser.Fields.From].Value;
                    var emailTemplateItem = notifyStepItem.TargetItem(Templates.NotifyNextStepUser.Fields.EmailTemplate);
                    var receivers = GetReceivers(contentItem, currentWorkflowStep.ID, nextWorkflowStep.ID);

                    _workflowNotifyService.SendEmailNotifications(receivers, from, contentItem, workflowComment, emailTemplateItem);
                }
            }
            catch (Exception ex)
            {
                Log.Error($"NotifyNextStepUser: {ex}", this);
            }
        }

        private IEnumerable<User> GetReceivers(Item contentItem, ID currentState, ID nextState)
        {
            if (currentState.Equals(Constants.States.Draft) && nextState.Equals(Constants.States.WaitingForMallSiteApproval))
            {
                return WorkflowHelper.GetMallAdminsOf(contentItem.GetSiteItem());
            }
            else if (currentState.Equals(Constants.States.Draft) && nextState.Equals(Constants.States.WaitingForMainSiteApproval))
            {
                return WorkflowHelper.GetMainAdminsOf(contentItem.GetSiteItem());
            }
            else if ((currentState.Equals(Constants.States.WaitingForMallSiteApproval) && nextState.Equals(Constants.States.Draft))
               || (currentState.Equals(Constants.States.WaitingForMainSiteApproval) && nextState.Equals(Constants.States.Draft))
               || (currentState.Equals(Constants.States.WaitingForMallSiteApproval) && nextState.Equals(Constants.States.Approved))
               || (currentState.Equals(Constants.States.WaitingForMainSiteApproval) && nextState.Equals(Constants.States.Approved)))
            {
                var result = new List<User>
                {
                    WorkflowHelper.GetAuthorOf(contentItem)
                };

                var sourceItem = _mallSiteWorkflowRepository.TryGetSourceItemBy(contentItem);
                if (sourceItem != null) // Get related user if existed.
                {
                    result.Add(User.FromName(sourceItem.Statistics.UpdatedBy, true));
                }
                return result.Distinct();
            }
            return Enumerable.Empty<User>();
        }
    }

}