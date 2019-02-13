using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Foundation.Multisite.Extensions;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Security.Accounts;
using Sitecore.Workflows.Simple;

namespace Sitecore.Foundation.Workflow.Helpers
{
    public static class WorkflowHelper
    {
        public static Item GetCurrentState(WorkflowPipelineArgs args) 
            => args.ProcessorItem.InnerItem.Parent.Parent;
        public static Item GetNextState(WorkflowPipelineArgs args)
        {
            Item command = args.ProcessorItem.InnerItem.Parent;
            string nextStateId = command[Constants.NextState];

            if (nextStateId.Length == 0)
            {
                return null;
            }

            Item nextState = args.DataItem.Database.Items[ID.Parse(nextStateId)];

            if (nextState != null)
            {
                return nextState;
            }

            return null;
        }

        public static bool DoesItemHasPresentationDetails(string itemId)
        {
            if (Sitecore.Data.ID.IsID(itemId))
            {
                Item item = Sitecore.Context.Database.GetItem(Sitecore.Data.ID.Parse(itemId));
                if (item != null)
                {
                    return item.Fields[Sitecore.FieldIDs.LayoutField] != null && item.Fields[Sitecore.FieldIDs.LayoutField].HasValue;
                }
            }
            return false;
        }
        public static IEnumerable<User> GetMainAdminsOf(Item site) => GetUsersOfSiteByRole(site, Constants.Role.MainAdmin);
        public static IEnumerable<User> GetMallAdminsOf(Item site) => GetUsersOfSiteByRole(site, Constants.Role.MallAdmin);
        public static IEnumerable<User> GetUsersOfSiteByRole(Item site, string roleName)
        {
            var result = new List<User>();
            var accessRules = site.Security.GetAccessRules()?
                .Where(a => a.Account.AccountType.Equals(AccountType.User));
            if (accessRules != null && accessRules.Any())
            {
                var accessUsersInRole = accessRules.Select(a => ((User)a.Account))
                    .Where(u => u.IsInRole(Role.FromName(roleName)));
                result.AddRange(accessUsersInRole);
            }
            return result.Distinct();
        }
        public static User GetAuthorOf(Item contentItem)
        {
            return User.FromName(contentItem.Statistics.CreatedBy, true);
        }
    }
}