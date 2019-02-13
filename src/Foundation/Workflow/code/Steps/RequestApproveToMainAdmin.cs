using Sitecore.Data.Items;
using Sitecore.DependencyInjection;
using Sitecore.Diagnostics;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Foundation.Workflow.Helpers;
using Sitecore.Foundation.Workflow.Services;
using Sitecore.SecurityModel;
using Sitecore.Workflows.Simple;
using System;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.Foundation.Multisite.Extensions;
using Sitecore.Foundation.Workflow.Repositories;

namespace Sitecore.Foundation.Workflow.Steps
{
    // this step is cross site. Can't using the NotifyNextStepUser
    public class RequestApproveToMainAdmin
    {
        private readonly IWorkflowNotifyService _workflowNotifyService;
        private readonly IMallSiteWorkflowRepository _mallSiteWorkflowRepository;

        public RequestApproveToMainAdmin()
        {
            _workflowNotifyService = ServiceLocator.ServiceProvider.GetService<IWorkflowNotifyService>();
            _mallSiteWorkflowRepository = ServiceLocator.ServiceProvider.GetService<IMallSiteWorkflowRepository>();
        }
        public void Process(WorkflowPipelineArgs args)
        {
            string workflowComment = (!string.IsNullOrEmpty(args.CommentFields[Constants.Comments])) ? args.CommentFields[Constants.Comments] : Constants.NoComment;
            try
            {
                using (new SecurityDisabler())
                {
                    Item currentDataItem = args.DataItem;
                    if (currentDataItem.Fields[Multisite.Templates.MainSite.Fields.IsDisplayOnMainSite].IsChecked()) 
                    {
                        Item copiedItem = _mallSiteWorkflowRepository.GetCopiedItemInMainSite(currentDataItem);
                        Item notifyStepItem = args.ProcessorItem.InnerItem;

                        var from = notifyStepItem.Fields[Templates.NotifyNextStepUser.Fields.From].Value;
                        var emailTemplateItem = notifyStepItem.TargetItem(Templates.NotifyNextStepUser.Fields.EmailTemplate);
                        var receivers = WorkflowHelper.GetMainAdminsOf(copiedItem.GetSiteItem());

                        _workflowNotifyService.SendEmailNotifications(receivers, from, copiedItem, workflowComment, emailTemplateItem);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"NotifyNextStepUser: {ex}", this);
            }
        }
    }
}