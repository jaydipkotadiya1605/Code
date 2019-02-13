namespace Sitecore.Foundation.Workflow.CustomEvents
{
    using Sitecore.Data;
    using Sitecore.Data.Fields;
    using Sitecore.Data.Items;
    using Sitecore.Diagnostics;
    using Sitecore.Events;
    using Sitecore.Foundation.Multisite;
    using Sitecore.Foundation.Multisite.Extensions;
    using Sitecore.StringExtensions;
    using System;

    public class DeleteItemEventHandle 
    {
        public void OnDeletingItemMultisite(object sender, EventArgs args)
        {
            var deletingItem = Event.ExtractParameter(args, 0) as Item;
            Error.AssertObject(deletingItem, "Item");

            if (deletingItem != null && deletingItem.IsBelongToMainSite())
            {
                this.DeleteItemOnMainSite(deletingItem);
            }

            if (deletingItem != null && deletingItem.IsBelongToMallSite())
            {
                this.DeleteItemOnMallSite(deletingItem);
            }
        }
        private void DeleteItemOnMainSite(Item deletingItem)
        {
            var id = deletingItem.Fields[HiddenFields.Templates.HiddenField.Fields.SourceId].Value;
            if (id.IsNullOrEmpty()) return;
            var referenceItem = deletingItem.Database.GetItem(new ID(id));
            if (referenceItem != null && referenceItem.IsBelongToMallSite())
            {
                using (new Sitecore.SecurityModel.SecurityDisabler())
                {
                    referenceItem.Editing.BeginEdit();
                    try
                    {
                        referenceItem.Fields[HiddenFields.Templates.HiddenField.Fields.TargetIds].Value = string.Empty;
                    }
                    catch (Exception ex)
                    {
                        Log.Error($"Could not set source ID | {ex.Message}", nameof(DeleteItemEventHandle));
                        referenceItem.Editing.CancelEdit();
                    }
                    referenceItem.Editing.EndEdit();
                }

            }
        }
        private void DeleteItemOnMallSite(Item deletingItem)
        {
            var id = deletingItem.Fields[HiddenFields.Templates.HiddenField.Fields.SourceId].Value;
            if (id.IsNullOrEmpty()) return;
            var referenceItem = deletingItem.Database.GetItem(new ID(id));

            if (referenceItem != null && referenceItem.IsBelongToMainSite())
            {
                using (new Sitecore.SecurityModel.SecurityDisabler())
                {
                    referenceItem.Editing.BeginEdit();
                    try
                    {
                        var mallSiteId = deletingItem.GetSiteItem().ID;
                        NameValueListField targetIds = referenceItem.Fields[HiddenFields.Templates.HiddenField.Fields.TargetIds];
                        var tagets = targetIds.NameValues;
                        tagets.Remove(mallSiteId.ToShortID().ToString());
                        targetIds.NameValues = tagets;

                        ((MultilistField)referenceItem.Fields[Templates.MallSite.Fields.SiteDisplaySettings]).Remove(mallSiteId.ToString());
                    }
                    catch (Exception ex)
                    {
                        Log.Error($"Could not set source ID | {ex.Message}", nameof(DeleteItemEventHandle));
                        referenceItem.Editing.CancelEdit();
                    }
                    referenceItem.Editing.EndEdit();
                }

            }
        }
    }
}