using System;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.SecurityModel;
using Sitecore.Workflows.Simple;
using Sitecore.Foundation.SitecoreExtensions.Extensions;

namespace Sitecore.Foundation.Workflow.Steps
{
    public class SubmitToMallContentStep
    {
        public void Process(WorkflowPipelineArgs args)
        {
            Item contentItem = args.DataItem;
            try
            {
                if (((Sitecore.Data.Fields.CheckboxField)contentItem.Fields[Multisite.Templates.MainSite.Fields.IsDisplayOnMainSite]).Checked)
                {
                    using (new SecurityDisabler())
                    {

                        // Correct data when: Wrong user behavior. 
                        // => User checked "Display on main" => "Send to mall" ONLY => uncheck  "Display on main" 
                        contentItem.Editing.BeginEdit();
                        ((Sitecore.Data.Fields.CheckboxField)contentItem.Fields[Multisite.Templates.MainSite.Fields.IsDisplayOnMainSite]).Checked = false;
                        contentItem.Editing.EndEdit();
                    }
                }
                
            }
            catch (Exception ex)
            {
                Log.Error($"SubmitToMallContentStep: {ex}", this);
            }
        }
    }
}