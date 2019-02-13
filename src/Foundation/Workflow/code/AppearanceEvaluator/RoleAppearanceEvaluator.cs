using Sitecore.Data.Items;
using Sitecore.Workflows;

namespace Sitecore.Foundation.Workflow.AppearanceEvaluator
{
    public class RoleAppearanceEvaluator : BasicWorkflowCommandAppearanceEvaluator
    {
        public override WorkflowCommandState EvaluateState(Item item, Item workflowCommand)
        {
            // show/hide based on roles
            if (Sitecore.Context.User.IsInRole(Constants.Role.MallAdmin))
            {
                return WorkflowCommandState.Hidden;
            }
            else
            {
                return base.EvaluateState(item, workflowCommand);
            }
        }
    }
}