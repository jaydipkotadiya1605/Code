namespace Sitecore.Foundation.Workflow.Repositories
{
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.Buckets.Managers;
    using Sitecore.Data;
    using Sitecore.Data.Fields;
    using Sitecore.Data.Items;
    using Sitecore.DependencyInjection;
    using Sitecore.Exceptions;
    using Sitecore.Foundation.DependencyInjection;
    using Sitecore.Foundation.Multisite;
    using Sitecore.Foundation.Multisite.Extensions;
    using Sitecore.Foundation.Multisite.Model;
    using Sitecore.Foundation.SitecoreExtensions.Extensions;
    using Sitecore.StringExtensions;
    using System;
    using System.Collections.Specialized;

    [Service(typeof(IMainSiteWorkflowRepository))]
    public class MainSiteWorkflowRepository : IMainSiteWorkflowRepository
    {
        private readonly IStateRepository stateRepository;
        public MainSiteWorkflowRepository()
        {
            this.stateRepository = ServiceLocator.ServiceProvider.GetService<IStateRepository>();
        }
        public void CloneOrRemoveItemsInMalls(Item sourceItem)
        {
            if (!sourceItem.IsDerived(Templates.MallSite.ID)) throw new TemplateNotFoundException();
            var mainSite = sourceItem.GetSiteItem();
            MultilistField mallSiteSetting = mainSite.Fields[Templates.MainSiteSetting.Fields.MallSites];
            if (mallSiteSetting.Value.IsNullOrEmpty())
            {
                throw new ClientAlertException("Mall Site Setting is not correct");
            }
            MultilistField mallSiteInput = sourceItem.Fields[Templates.MallSite.Fields.SiteDisplaySettings];
            NameValueListField targetIds = sourceItem.Fields[HiddenFields.Templates.HiddenField.Fields.TargetIds];
            var setting = new MultisiteSettingCollection(targetIds.NameValues, mallSiteSetting.GetItems(), mallSiteInput.GetItems());
            var targetIdsResult = targetIds.NameValues;

            foreach (var site in setting.CloneSites)
            {
                this.CloneItem(sourceItem, site, targetIdsResult);
            }

            foreach (var removeItem in setting.RemoveItemIds)
            {
                var item = sourceItem.Database.GetItem(removeItem.DeleteItemId);
                item.Delete();
                targetIdsResult.Remove(removeItem.SiteId.ToShortID().ToString());
            }
            targetIds.NameValues = targetIdsResult;
        }

        public void ChangeStateItemsInMalls(Item sourceItem, ID state)
        {
            if (!sourceItem.IsDerived(Templates.MallSite.ID)) throw new TemplateNotFoundException();
            NameValueListField targetIds = sourceItem.Fields[HiddenFields.Templates.HiddenField.Fields.TargetIds];
            if (targetIds == null) return;
            foreach (string key in targetIds.NameValues)
            {
                var item = sourceItem.Database.GetItem(new ID(targetIds.NameValues[key]));
                this.stateRepository.ChangeItemStateTo(item, state);
            }

        }

        private void CloneItem(Item sourceItem, Item destinatonSite, NameValueCollection targets)
        {
            var destination = sourceItem.GetDestinationItem(destinatonSite);
            if (destination == null)
            {
                throw new ClientAlertException("Parent Item on Mall Site is not exists.");
            }
            var newItem = BucketManager.IsItemContainedWithinBucket(sourceItem)
                ? BucketManager.CloneItem(sourceItem, destination, true)
                : sourceItem.CloneTo(destination);
            targets.Add(newItem.GetSiteItem().ID.ToShortID().ToString(), newItem.ID.Guid.ToString().ToUpper());
            newItem.Editing.BeginEdit();
            newItem.Fields[HiddenFields.Templates.HiddenField.Fields.SourceId].SetValue(sourceItem.ID.ToString(), true);
            newItem.Fields[Templates.MainSiteSetting.Fields.MallSites].SetValue(string.Empty, true);
            newItem.Editing.EndEdit();
        }
    }
}