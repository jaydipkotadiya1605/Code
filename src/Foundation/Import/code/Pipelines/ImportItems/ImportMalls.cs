using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Foundation.Import.Extensions;
using Sitecore.Foundation.Import.Map;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Sitecore.Foundation.Import.Pipelines.ImportItems
{
    public class ImportMalls : ImportItemsProcessor
    {

        public override void Process(ImportItemsArgs args)
        {
            List<string> mallNames;
            if (args.ContentType == ContentType.Store.ToString())
            {
                mallNames = GetMallName(args.ImportDatas[0]);
            }
            else
            {
                mallNames = new DescriptionAttributes<Malls>().Descriptions.ToList();
            }
            args.MallItems = CreateMallItems(mallNames, args);
        }

        private List<string> GetMallName(DataTable table)
        {
            var malls = table.AsEnumerable().Select(r => r.Field<string>(StoreInputField.Mall.ToString())).Where( x => !string.IsNullOrEmpty(x)).Distinct().ToList();
            return malls;
        }

        private List<MallItem> CreateMallItems(List<string> malls, ImportItemsArgs args)
        {
            var templateItem = args.Database.GetTemplate(Templates.MallWebsite.ID);

            //get the parent in the specific language
            Item parent = args.Database.GetItem(Templates.Content.ID);

            var children = parent.GetChildren();
            List<MallItem> newItems = new List<MallItem>();
            foreach (string mallName in malls)
            {
                var mallItem = new MallItem();
                Item newItem;
                //search for the child by name
                newItem = children[mallName];
                if (newItem != null)
                {
                    UpdateOrGetMallInfo(args, newItem, mallItem, newItems);
                }
                else
                {
                    //if not found then create one
                    args.Statistics.CreatedItems++;
                    newItem = parent.Add(mallName, templateItem);
                    Log.Info(string.Format("Sitecore.Foundation.Import:Creating item {0}", newItem.Paths.ContentPath), this);
                }

                AssignMainSiteforMallAndContratry(newItem, args);
                mallItem = CreateSubFolderForMall(args, mallItem, newItem);

                if (args.ContentType == ContentType.ContactUs.ToString())
                    UpdateContactUsEachMall(newItem, args.ImportDatas[0], args);
                newItems.Add(mallItem);
            }

            return newItems;
        }

        private void AssignMainSiteforMallAndContratry(Item mallItem, ImportItemsArgs args)
        {
            var fraserRewardItem = args.Database.GetItem(Templates.FrasersRewards.ID);
            if (fraserRewardItem != null)
            {
                using (new EditContext(fraserRewardItem, true, false))
                {
                    var mallSites = fraserRewardItem.Fields[Templates.FrasersRewards.MallSites].Value;
                    if (!mallSites.Contains(mallItem.ID.ToString()))
                    {
                        if (string.IsNullOrEmpty(fraserRewardItem.Fields[Templates.FrasersRewards.MallSites].Value))
                            fraserRewardItem.Fields[Templates.FrasersRewards.MallSites].Value = mallSites + mallItem.ID.ToString();
                        else fraserRewardItem.Fields[Templates.FrasersRewards.MallSites].Value = mallSites + "|" + mallItem.ID.ToString();
                    }
                }

                using (new EditContext(mallItem, true, false))
                {
                    mallItem.Fields[Templates.MainSiteField].Value = Templates.FrasersRewards.ID.ToString();
                }
            }
        }

        private MallItem CreateSubFolderForMall(ImportItemsArgs args, MallItem mallItem, Item newItem)
        {
            // Create Local Content Folder
            var templateLocalContentFolderItem = args.Database.GetTemplate(Templates.LocalContentFolder.ID);
            var localFolderItem = CreateItem(args, templateLocalContentFolderItem, newItem, Templates.LocalContentFolder.DefaultName);

            // Create Banners Folder
            var templateBannerFolderItem = args.Database.GetTemplate(Templates.BannerFolder.ID);
            var bannerFolderItem = CreateItem(args, templateBannerFolderItem, localFolderItem, Templates.BannerFolder.DefaultName);

            // Create Blogs Folder
            var templateBlogFolderItem = args.Database.GetTemplate(Templates.BlogFolder.ID);
            var blogFolderItem = CreateItem(args, templateBlogFolderItem, localFolderItem, Templates.BlogFolder.DefaultName);

            // Create Articles Folder
            var templateArticleFolderItem = args.Database.GetTemplate(Templates.ArticleFolder.ID);
            var articleFolderItem = CreateItem(args, templateArticleFolderItem, localFolderItem, Templates.ArticleFolder.DefaultName);

            // Create Events Folder
            var templateEventFolderItem = args.Database.GetTemplate(Templates.EventFolder.ID);
            var eventFolderItem = CreateItem(args, templateEventFolderItem, localFolderItem, Templates.EventFolder.DefaultName);

            // Create Pages Folder
            var templatePageFolderItem = args.Database.GetTemplate(Templates.ContentFolder.ID);
            var pageFolderItem = CreateItem(args, templatePageFolderItem, localFolderItem, Templates.ContentFolder.DefaultName);

            // Create Stores Folder
            var templateStoreFolderItem = args.Database.GetTemplate(Templates.StoreFolder.ID);
            var storeFolderItem = CreateItem(args, templateStoreFolderItem, localFolderItem, Templates.StoreFolder.DefaultName);


            mallItem.MallName = newItem.Name;
            mallItem.MallID = newItem.ID;
            mallItem.StoreRootID = storeFolderItem.ID;
            mallItem.BannerRootID = bannerFolderItem.ID;
            mallItem.EventRootID = eventFolderItem?.ID;
            mallItem.ArticleRootID = articleFolderItem.ID;
            mallItem.BlogRootID = blogFolderItem.ID;
            mallItem.PageRootID = pageFolderItem?.ID;

            return mallItem;
        }

        private void UpdateOrGetMallInfo(ImportItemsArgs args, Item newItem, MallItem mallItem, List<MallItem> newItems)
        {
            if (args.ImportOptions.ExistingItemHandling == ExistingItemHandling.AddVersion)
            {
                args.Statistics.UpdatedItems++;
                newItem = newItem.Versions.AddVersion();
                Log.Info(string.Format("Sitecore.Foundation.Import:Creating new version of item {0}", newItem.Paths.ContentPath),
                    this);
            }
            else if (args.ImportOptions.ExistingItemHandling == ExistingItemHandling.Skip)
            {
                Log.Info(string.Format("Sitecore.Foundation.Import:Skipping update of item {0}", newItem.Paths.ContentPath), this);

                var storeFolder = newItem.GetChildren()[Templates.StoreFolder.DefaultName];
                var bannerFolder = newItem.GetChildren()[Templates.BannerFolder.DefaultName];
                var eventFolder = newItem.GetChildren()[Templates.EventFolder.DefaultName];
                var articleFolder = newItem.GetChildren()[Templates.ArticleFolder.DefaultName];
                var blogFolder = newItem.GetChildren()[Templates.BlogFolder.DefaultName];
                var pageFolder = newItem.GetChildren()[Templates.ContentFolder.DefaultName];

                mallItem.MallName = newItem.Name;
                mallItem.StoreRootID = storeFolder?.ID;
                mallItem.BannerRootID = bannerFolder?.ID;
                mallItem.EventRootID = eventFolder?.ID;
                mallItem.ArticleRootID = articleFolder?.ID;
                mallItem.BlogRootID = blogFolder?.ID;
                mallItem.PageRootID = pageFolder?.ID;
                mallItem.MallID = newItem.ID;
                if (args.ContentType == ContentType.ContactUs.ToString())
                    UpdateContactUsEachMall(newItem, args.ImportDatas[0], args);
                newItems.Add(mallItem);
            }
            else if (args.ImportOptions.ExistingItemHandling == ExistingItemHandling.Update)
            {
                //continue to update current item/version
                args.Statistics.UpdatedItems++;
            }
        }

        private Item CreateItem(ImportItemsArgs args, TemplateItem item, Item parentItem, string itemName = "")
        {
            var templateItem = args.Database.GetTemplate(item.ID);

            //get the parent in the specific language
            Item parent = args.Database.GetItem(parentItem.ID);

            if (string.IsNullOrEmpty(itemName))
            {
                itemName = item.Name;
            }

            Item newItem = Utils.SearchChildItem(args, parent, itemName, templateItem, this);

            return newItem;
        }

        private void UpdateContactUsEachMall(Item mallItem, DataTable dataTable, ImportItemsArgs args)
        {
            using (new EditContext(mallItem, true, false))
            {
                var dataRow = dataTable.AsEnumerable().FirstOrDefault(row => row[ContactUsInputField.Mall.ToString()].ToString().Equals(mallItem.Name, StringComparison.OrdinalIgnoreCase));
                if (dataRow != null)
                {
                    mallItem.Fields[ContactUsOutField.Address.GetDescription()].Value = dataRow[ContactUsInputField.Address.ToString()].ToString();
                    mallItem.Fields[ContactUsOutField.Latitude.GetDescription()].Value = dataRow[ContactUsInputField.Latitude.ToString()].ToString();
                    mallItem.Fields[ContactUsOutField.Longitude.GetDescription()].Value = dataRow[ContactUsInputField.Longitude.ToString()].ToString();
                    mallItem.Fields[ContactUsOutField.Telephone.GetDescription()].Value = dataRow[ContactUsInputField.ContactNo.ToString()].ToString();
                    mallItem.Fields[ContactUsOutField.OpeningHrs.GetDescription()].Value = dataRow[ContactUsInputField.OpeningHrs.ToString()].ToString();
                    mallItem.Fields[ContactUsOutField.Details.GetDescription()].Value = dataRow[ContactUsInputField.Deatils.ToString()].ToString();

                    var photoItem = GetOrCreateAndgetPhotoItem(args, dataRow);
                    if (photoItem != null)
                    {
                        mallItem.Fields[ContactUsOutField.Photo.GetDescription()].Value = string.Format("<image mediaid=\"{0}\"/>", photoItem.ID.ToString());
                    }

                    var leasingResources = GetLeasingResources(dataRow);
                    if (!string.IsNullOrEmpty(leasingResources))
                        mallItem.Fields[ContactUsOutField.LeasingResources.GetDescription()].Value = leasingResources;
                }
            }
        }

        private Item GetOrCreateAndgetPhotoItem(ImportItemsArgs args, DataRow dataRow)
        {
            var photoNameWithoutExtention = Utils.GetFileNoExtension(dataRow[ContactUsInputField.ContactNo.ToString()].ToString());
            var slashString = Sitecore.Configuration.Settings.GetSetting("Sitecore.Foundation.Import.Slash");
            var photoItem = args.Database.GetItem(Constants.IdentityImageSiteCorePath + slashString + photoNameWithoutExtention);
            if (photoItem == null)
            {
                photoItem = args.Database.GetItem(Constants.UploadedSiteCoreSiteCorePath + slashString + photoNameWithoutExtention);
            }

            return photoItem;
        }

        private string GetLeasingResources(DataRow dataRow)
        {
            var leasingResources = string.Empty;
            if (dataRow[ContactUsInputField.GotAtrium.ToString()].ToString() == Constants.ChoosedValue)
            {
                leasingResources = Templates.LeasingResources.Atrium.ToString();
            }
            if (dataRow[ContactUsInputField.GotPushCart.ToString()].ToString() == Constants.ChoosedValue)
            {
                if (string.IsNullOrEmpty(leasingResources))
                    leasingResources = Templates.LeasingResources.PushCart.ToString();
                else
                    leasingResources += "|" + Templates.LeasingResources.PushCart.ToString();
            }
            if (dataRow[ContactUsInputField.GotShopSpace.ToString()].ToString() == Constants.ChoosedValue)
            {
                if (string.IsNullOrEmpty(leasingResources))
                    leasingResources = Templates.LeasingResources.ShopSpace.ToString();
                else
                    leasingResources += "|" + Templates.LeasingResources.ShopSpace.ToString();
            }

            return leasingResources;
        }
    }
}