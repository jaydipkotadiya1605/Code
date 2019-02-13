using Sitecore.Data.Items;
using Sitecore.Security.Accounts;
using System.Collections.Generic;

namespace Sitecore.Foundation.Workflow.Services
{
    public interface IWorkflowNotifyService
    {
        void SendEmailNotifications(IEnumerable<User> receivers,string from, Item contentItem, string comment, Item emailTemplateItem);
    }
}
