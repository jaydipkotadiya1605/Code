namespace Sitecore.Foundation.Multisite.CustomEvents
{
    using System;
    using Sitecore.Data.Items;
    using Sitecore.Events;
    using Sitecore.Foundation.Multisite.Extensions;
    using Sitecore.Foundation.SitecoreExtensions.Extensions;

    public class CopyItemHasHiddenFieldsEvent
    {
        public void OnItemCopied(object sender, EventArgs args)
        {
            Item sourceItem = Event.ExtractParameter(args, 0) as Item;
            Item resultItem = Event.ExtractParameter(args, 1) as Item;
            if (sourceItem == null || resultItem == null) return;
            if (sourceItem.IsDerived(HiddenFields.Templates.HiddenField.ID) && resultItem.IsDerived(HiddenFields.Templates.HiddenField.ID))
            {
                var siteOfSourceItem = sourceItem.GetSiteItem();
                var siteOfResultItem = sourceItem.GetSiteItem();
                if (siteOfResultItem == null || siteOfSourceItem == null)
                    return;
                if (siteOfSourceItem.ID.Equals(siteOfResultItem.ID))
                {
                    resultItem.Editing.BeginEdit();
                    resultItem.Fields[HiddenFields.Templates.HiddenField.Fields.SourceId].Value = string.Empty;
                    resultItem.Fields[HiddenFields.Templates.HiddenField.Fields.TargetIds].Value = string.Empty;
                    resultItem.Fields[Templates.MallSite.Fields.SiteDisplaySettings].Value = null;
                    resultItem.Fields[Templates.MainSite.Fields.IsDisplayOnMainSite].Value = null;
                    resultItem.Editing.EndEdit();
                }
            }
        }
    }
}