using Sitecore.ContentSearch.Utilities;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Foundation.Abstractions.SitecoreContext;
using Sitecore.Foundation.Import.Extensions;
using Sitecore.Foundation.Import.Map;
using Sitecore.Foundation.Search.Repositories;
using FrasersContent = Sitecore.Foundation.FrasersContent;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;

namespace Sitecore.Foundation.Import.Pipelines.ImportItems
{
    public class BuildImportDataStructure : ImportItemsProcessor
    {
        public override void Process(ImportItemsArgs args)
        {
            var rootItem = new ItemDto("<root>"); //ick
            if (args.ContentType != ContentType.Article.ToString())
            {
                ItemImportMap map = null;
                if (args.ContentType == ContentType.Store.ToString())
                {
                    var mapItem = Factory.SetMapInputField(ContentType.Store.ToString(), Templates.Store.ID);
                    map = Factory.BuildMapInfo(mapItem, Templates.Store.ID, ContentType.Store);

                }
                else if (args.ContentType == ContentType.Banner.ToString())
                {
                    var mapItem = Factory.SetMapInputField(ContentType.Banner.ToString(), Templates.Banner.ID);
                    map = Factory.BuildMapInfo(mapItem, Templates.Banner.ID, ContentType.Banner);
                }
                else if (args.ContentType == ContentType.Event.ToString())
                {
                    var mapItem = Factory.SetMapInputField(ContentType.Event.ToString(), Templates.Event.ID);
                    map = Factory.BuildMapInfo(mapItem, Templates.Event.ID, ContentType.Event);
                }
                else if (args.ContentType == ContentType.Blog.ToString())
                {
                    var mapItem = Factory.SetMapInputField(ContentType.Blog.ToString(), FrasersContent.Templates.Blog.ID);
                    map = Factory.BuildMapInfo(mapItem, FrasersContent.Templates.Blog.ID, ContentType.Blog);
                }

                if (map != null)
                    args.Map.Add(map);
                MapImportData(map, args, rootItem, args.ImportDatas[0]);
            }
            else
            {
                var mapArticleItem = Factory.SetMapInputField(ContentType.Article.ToString(), Templates.Article.ID);
                var articleMap = Factory.BuildMapInfo(mapArticleItem, Templates.Article.ID, ContentType.Article);
                args.Map.Add(articleMap);
                MapImportData(articleMap, args, rootItem, args.ImportDatas[0]);

                var mapSpecialItem = Factory.SetMapInputField(ContentType.Article.ToString(), FrasersContent.Templates.SpecialEvent.ID);
                var specialMap = Factory.BuildMapInfo(mapSpecialItem, FrasersContent.Templates.SpecialEvent.ID, ContentType.Article);
                args.Map.Add(specialMap);
                MapImportData(specialMap, args, rootItem, args.ImportDatas[1], true);
            }

            args.ImportItems.AddRange(rootItem.Children); //ick

        }

        private void MapImportData(ItemImportMap map, ImportItemsArgs args, ItemDto rootItem, DataTable dataImport, bool isSpecialEvent = false)
        {
            if (map != null)
            {
                foreach (var outputMap in map.OutputMaps)
                {
                    ImportMapItems(args, dataImport, outputMap, rootItem, true, isSpecialEvent); //ick
                }
            }
        }

        private void ImportMapItems(ImportItemsArgs args, DataTable dataTable, OutputMap outputMap, ItemDto parentItem,
            bool rootLevel, bool isSpecialEvent)
        {
            var groupedTable = dataTable.GroupBy(outputMap.Fields.Select(f => f.SourceColumn).ToArray());

            for (int i = 0; i < groupedTable.Rows.Count; i++)
            {
                var row = groupedTable.Rows[i];
                if (rootLevel ||
                    System.Convert.ToString(row[outputMap.ParentMap.NameInputField]) == parentItem.Name)
                {
                    ID rootItem = ID.Null;
                    var tableInfo = new TableInfo() { CurrentRow = i, Datatable = dataTable};
                    if (args.ContentType == ContentType.Banner.ToString())
                    {
                        BannerMapItems(args, outputMap, row, parentItem, tableInfo);
                    }
                    else if (args.ContentType == ContentType.Event.ToString())
                    {
                        EventMapItems(args, outputMap, row, parentItem, tableInfo);
                    }
                    else if (args.ContentType == ContentType.Article.ToString())
                    {
                        ArticleMapItems(args, outputMap, row, parentItem, tableInfo, isSpecialEvent);
                    }
                    else if(args.ContentType == ContentType.Store.ToString())
                    {
                        var rowMallName = dataTable.Rows[i][StoreInputField.Mall.ToString()].ToString();
                        rootItem = args.MallItems.Where(x => x.MallName.Equals(rowMallName, StringComparison.OrdinalIgnoreCase)).Select(x => x.StoreRootID).FirstOrDefault();
                        ImportMapEachRow(row, outputMap, args, rootItem, parentItem, tableInfo);
                    }
                    else if (args.ContentType == ContentType.Blog.ToString())
                    {
                        BlogMapItems(args, outputMap, row, parentItem, tableInfo);
                    }
                    else
                        ImportMapEachRow(row, outputMap, args, rootItem, parentItem, tableInfo);
                }
            }
        }

        private void BannerMapItems(ImportItemsArgs args, OutputMap outputMap, DataRow row, ItemDto parentItem, TableInfo tableInfo)
        {
            var showInMain = tableInfo.Datatable.Rows[tableInfo.CurrentRow][BannerInputField.ShowInMain.ToString()].ToString();
            if (showInMain == Constants.ShowInMainValue)
            {
                var rootItem = new ID(Constants.MainSiteBannerRootId);
                ImportMapEachRow(row, outputMap, args, rootItem, parentItem, tableInfo);
            }
            var showInMallValues = tableInfo.Datatable.Rows[tableInfo.CurrentRow][BannerInputField.ShowInMalls.ToString()].ToString();
            var mallItems = CreateAndGetMalls(showInMallValues, args);
            foreach (MallItem mallItem in mallItems)
            {
                ImportMapEachRow(row, outputMap, args, mallItem.BannerRootID, parentItem, tableInfo);
            }
        }

        private void EventMapItems(ImportItemsArgs args, OutputMap outputMap, DataRow row, ItemDto parentItem, TableInfo tableInfo)
        {
            // Show in Main
            var rootItem = new ID(Constants.MainSiteEventRootId);
            ImportMapEachRow(row, outputMap, args, rootItem, parentItem, tableInfo);

            //Show in Malls
            var showInMallValues = tableInfo.Datatable.Rows[tableInfo.CurrentRow][EventInputField.ShowInMalls.ToString()].ToString();
            var mallItems = CreateAndGetMalls(showInMallValues, args);
            foreach (MallItem mallItem in mallItems)
            {
                ImportMapEachRow(row, outputMap, args, mallItem.EventRootID, parentItem, tableInfo);
            }
        }

        private void ArticleMapItems(ImportItemsArgs args, OutputMap outputMap, DataRow row, ItemDto parentItem, TableInfo tableInfo, bool isSpecialEvent)
        {
            var dataTable = tableInfo.Datatable;
            var currentRow = tableInfo.CurrentRow;

            // Show in Main
            var rootItem = new ID(Constants.MainSiteArticleRootId);
            if (isSpecialEvent)
            {
                rootItem = new ID(Constants.MainSiteEventRootId);
            }
            args.CurrentImportMallIdTemp = string.Empty;
            ImportMapEachRow(row, outputMap, args, rootItem, parentItem, tableInfo, isSpecialEvent);

           // Show in Malls
            var showInMallValues = dataTable.Rows[currentRow][EventInputField.ShowInMalls.ToString()].ToString();
            var mallItems = CreateAndGetMalls(showInMallValues, args);
            foreach (MallItem mallItem in mallItems)
            {
                var rootArticleItem = mallItem.ArticleRootID;
                if (isSpecialEvent)
                {
                    rootArticleItem = mallItem.EventRootID;
                }
                args.CurrentImportMallIdTemp = mallItem.MallID.ToString();
                ImportMapEachRow(row, outputMap, args, rootArticleItem, parentItem, tableInfo, isSpecialEvent);
            }
        }

        private void BlogMapItems(ImportItemsArgs args, OutputMap outputMap, DataRow row, ItemDto parentItem, TableInfo tableInfo)
        {
            // Show in Main
            var rootItem = new ID(Constants.MainSiteBlogRootId);
            ImportMapEachRow(row, outputMap, args, rootItem, parentItem, tableInfo);

            //Show in Malls
            foreach (MallItem mallItem in args.MallItems)
            {
                ImportMapEachRow(row, outputMap, args, mallItem.BlogRootID, parentItem, tableInfo);
            }
        }

        private void ImportMapEachRow(DataRow dataRow, OutputMap outputMap, ImportItemsArgs args, ID rootItem, ItemDto parentItem, TableInfo tableInfo, bool isSpecialEvent = false)
		{
            var createdItem = CreateItemDto(dataRow, outputMap, args, tableInfo.Datatable, tableInfo.CurrentRow);
            if (createdItem != null)
            {
                createdItem.ParentRootId = rootItem;

                createdItem.Parent = parentItem;
                parentItem.Children.Add(createdItem);
                if (outputMap.ChildMaps != null
                    && outputMap.ChildMaps.Any())
                {
                    foreach (var childMap in outputMap.ChildMaps)
                    {
                        ImportMapItems(args, tableInfo.Datatable, childMap, createdItem, false, isSpecialEvent);
                    }
                }
            }
        }

        private ItemDto CreateItemDto(DataRow dataRow, OutputMap outputMap, ImportItemsArgs args, DataTable dataTable, int currentRow)
        {
            var item = InitialItem(outputMap.TemplateId, dataRow, args.ContentType);
            if(item != null)
                {
                for (int i = 0; i < outputMap.Fields.Count; i++)
                {
                    var mapFieldName = outputMap.Fields[i].TargetFieldName;
                    var fieldValue = GetFieldValue(dataRow, outputMap, args, dataTable, currentRow, i, mapFieldName);
                    item.Fields.Add(mapFieldName, fieldValue);
                }
            }
            return item;
        }

        private ItemDto InitialItem(ID TemplateId, DataRow dataRow, string contentType)
        {
            var itemName = string.Empty;
            var displayName = string.Empty;
            if (contentType == ContentType.Store.ToString())
            {
                displayName = dataRow[StoreInputField.Store.ToString()].ToString();
                itemName = Utils.GetValidItemName(displayName);
            }
            else
            {
                displayName = dataRow[BannerInputField.Title.ToString()].ToString();
                itemName = Utils.GetValidItemName(displayName);

            }
            ItemDto item = null;
            if (itemName != Constants.UnNamed)
            {
                item = new ItemDto(itemName)
                {
                    TemplateId = TemplateId,
                    DisplayName = displayName
                };
            }

            return item;
        }

        private string GetFieldValue(DataRow dataRow, OutputMap outputMap, ImportItemsArgs args, DataTable dataTable, int currentRow, int fieldIndex, string mapFieldName)
        {
            string fieldValue = string.Empty;
            if (!string.IsNullOrEmpty(mapFieldName))
            {
                fieldValue = dataRow[outputMap.Fields[fieldIndex].SourceColumn].ToString();
                if ((args.ContentType == ContentType.Event.ToString() && mapFieldName != EventOutputField.Description.GetDescription())
                    || (args.ContentType == ContentType.Article.ToString() && mapFieldName != ArticleOutputField.Description.GetDescription())
                    || (args.ContentType == ContentType.Blog.ToString() && mapFieldName != BlogOutputField.Body.ToString()))
                {
                    fieldValue = Utils.RemoveHTML(fieldValue);
                }


                if (args.ContentType == ContentType.Store.ToString())
                {
                    fieldValue = GetStoreFieldValue(args, dataTable, currentRow, mapFieldName, fieldValue);
                }
                else if (args.ContentType == ContentType.Banner.ToString())
                {
                    fieldValue = GetBannerFieldValue( args, mapFieldName, fieldValue);
                }
                else if (args.ContentType == ContentType.Event.ToString())
                {
                    fieldValue = GetEventFieldValue(args, mapFieldName, fieldValue);
                }
                else if (args.ContentType == ContentType.Article.ToString())
                {
                    fieldValue = GetArticleFieldValue(args, mapFieldName, fieldValue);
                }
                else if (args.ContentType == ContentType.Blog.ToString())
                {
                    fieldValue = GetBlogFieldValue(args, mapFieldName, fieldValue);
                }
            }

            return fieldValue;
        }

        private string GetStoreFieldValue(ImportItemsArgs args, DataTable dataTable, int currentRow,string mapFieldName, string fieldValue)
        {
            if (mapFieldName == StoreOutputField.Category.GetDescription())
            {
                fieldValue = CreateAndGetStoreCategoryId(fieldValue, args);
            }
            else if (mapFieldName == StoreOutputField.Logo.GetDescription())
            {
                fieldValue = GetMediaValue(args, Constants.StoreImageSiteCorePath, fieldValue);
            }
            else if (mapFieldName == StoreOutputField.StoreOffers.GetDescription())
            {
                fieldValue = GetStoreOffers(dataTable.Rows[currentRow]);
            }

            return fieldValue;
        }

        private string GetBannerFieldValue(ImportItemsArgs args, string mapFieldName, string fieldValue)
        {
            if (mapFieldName == BannerOutputField.ShowInMain.GetDescription() && fieldValue != Constants.ShowInMainValue)
            {
                fieldValue = string.Empty;
            }
            else if (mapFieldName == BannerOutputField.ShowInMalls.GetDescription())
            {
                fieldValue = CreateOrGetMallIDsString(args, fieldValue);
            }
            else if (mapFieldName == BannerOutputField.Image.GetDescription())
            {
                fieldValue = GetMediaValue(args, Constants.BannerImageSiteCorePath, fieldValue);
            }
            else if (mapFieldName == BannerOutputField.Link.GetDescription())
            {
                var link = fieldValue;
                fieldValue = string.Format("<link linktype=\"external\" url=\"{0}\" />", link);
            }

            return fieldValue;
        }

        private string GetEventFieldValue(ImportItemsArgs args, string mapFieldName, string fieldValue)
        {
            if (mapFieldName == EventOutputField.Image.GetDescription() || mapFieldName == EventOutputField.Thumbnail.GetDescription())
            {
                fieldValue = GetMediaValue(args, Constants.EventImageSiteCorePath, fieldValue);
            }
            else if (mapFieldName == EventOutputField.ShowInMalls.GetDescription())
            {
                fieldValue = CreateOrGetMallIDsString(args, fieldValue);
            }

            return fieldValue;
        }

        private string GetArticleFieldValue( ImportItemsArgs args, string mapFieldName, string fieldValue)
        {
            if (mapFieldName == ArticleOutputField.Banner.GetDescription() || mapFieldName == ArticleOutputField.Thumbnail.GetDescription())
            {
                fieldValue = GetMediaValue(args, Constants.ArticleImageSiteCorePath, fieldValue);
            }
            else if (mapFieldName == ArticleOutputField.ShowInMalls.GetDescription())
            {
                fieldValue = CreateOrGetMallIDsString(args, fieldValue);
            }
            else if (mapFieldName == ArticleOutputField.Category.GetDescription())
            {
                fieldValue = CreateAndGetArticleCategoryId(fieldValue, args);
            }
            else if (mapFieldName == ArticleOutputField.Store.GetDescription())
            {
                fieldValue = GetStoreForArticle(fieldValue, args.CurrentImportMallIdTemp, args);
            }

            return fieldValue;
        }

        private string GetBlogFieldValue(ImportItemsArgs args, string mapFieldName, string fieldValue)
        {
            if (mapFieldName == BlogOutputField.Thumbnail.GetDescription() 
                || mapFieldName == BlogOutputField.Banner.GetDescription())
            {
                var fileName = !string.IsNullOrEmpty(fieldValue) ? Path.GetFileName(fieldValue) : string.Empty;
                fieldValue = GetMediaValue(args, Constants.BlogImageSiteCorePath, fileName);
            }
            else if (mapFieldName == BlogOutputField.Category.GetDescription())
            {
                fieldValue = CreateAndGetBlogCategoryId(fieldValue, args);
            }

            return fieldValue;
        }

        private string CreateOrGetMallIDsString(ImportItemsArgs args, string fieldValue)
        {
            var malls = CreateAndGetMalls(fieldValue, args);
            StringBuilder strBuilder = new StringBuilder();
            if (malls.Count > 0)
            {
                foreach (MallItem mallItem in malls)
                {
                    if (string.IsNullOrEmpty(strBuilder.ToString()))
                        strBuilder.Append(mallItem.MallID.ToString());
                    else
                        strBuilder.Append("|" + mallItem.MallID.ToString());
                }
            }

            return strBuilder.ToString();
        }

        private string GetMediaValue(ImportItemsArgs args, string path, string fieldValue)
        {
            var fileNameNoExtention = Utils.GetFileNoExtension(fieldValue);
            var mediaItem = GetAndCopyFileItem(args, path, fileNameNoExtention);
            if (mediaItem != null)
            {
                fieldValue = string.Format("<image mediaid=\"{0}\"/>", mediaItem.ID.ToString());
            }
            return fieldValue;
        }

        private string CreateAndGetStoreCategoryId(string category, ImportItemsArgs args)
        {
            var separator = new[] { args.ImportOptions.MultipleValuesImportSeparator };
            var categoryValues = category != null
                    ? category.Split(separator, StringSplitOptions.RemoveEmptyEntries)
                    : new string[] { };
            StringBuilder result = new StringBuilder();
            foreach (var value in categoryValues)
            {
                var categoryEnum = Enum.GetValues(typeof(StoreCategory)).Cast<StoreCategory>().FirstOrDefault(x => ((int)x).ToString() == value);
                var categoryName = categoryEnum.ToString();
                var categoryDisplayName = categoryEnum.GetDescription();

                var templateItem = args.Database.GetTemplate(Templates.StoreCategories.ID);

                //get the parent in the specific language
                Item parent = args.Database.GetItem(Templates.StoreCategories.GlobalRoot);

                var newItem = CreateItem(parent, categoryName, categoryDisplayName, templateItem, args);

                if (string.IsNullOrEmpty(result.ToString()))
                    result.Append(newItem.ID.ToString());
                else result.Append("|" + newItem.ID.ToString());
            }
            return result.ToString();
        }

        private string CreateAndGetArticleCategoryId(string category, ImportItemsArgs args)
        {
            var categoryEnum = Enum.GetValues(typeof(ArticleCategory)).Cast<ArticleCategory>().FirstOrDefault(x => x.GetDescription() == category);
            var categoryName = categoryEnum.GetDescription();
            var categoryDisplayName = categoryEnum.GetDescription();

            var templateItem = args.Database.GetTemplate(Templates.ArticleCatecory.ID);

            //get the parent in the specific language
            Item parent = args.Database.GetItem(Templates.ArticleCatecory.GlobalRoot);
            var newItem = CreateItem(parent, categoryName, categoryDisplayName, templateItem, args);

            return newItem.ID.ToString();
        }

        private Item GetAndCopyFileItem(ImportItemsArgs args, string sitecorePath, string mediaItemName)
        {
            var slashString = Sitecore.Configuration.Settings.GetSetting("Sitecore.Foundation.Import.Slash");
            Item myItem = args.Database.GetItem(sitecorePath + slashString + mediaItemName);

            if (myItem != null)
            {
                return myItem;
            }
            else
            {
                myItem = args.Database.GetItem(Constants.UploadedSiteCoreSiteCorePath + slashString + mediaItemName);
                if (myItem != null)
                {
                    var folderItem = args.Database.GetItem(sitecorePath);
                    myItem.MoveTo(folderItem);
                    return myItem;
                }
                return null;
            }
        }

        private List<MallItem> CreateAndGetMalls(string showInMalls, ImportItemsArgs args)
        {
            var separator = new[] { args.ImportOptions.MultipleValuesImportSeparator };
            var mallValues = showInMalls != null
                    ? showInMalls.Split(separator, StringSplitOptions.RemoveEmptyEntries)
                    : new string[] { };
            List<MallItem> result = new List<MallItem>();
            foreach (var value in mallValues)
            {
                if (value != Constants.NAMall)
                {
                    var mallEnum = Enum.GetValues(typeof(Malls)).Cast<Malls>().FirstOrDefault(x => ((int)x).ToString() == value);
                    var mallName = mallEnum.ToString() != Constants.NAMall ? mallEnum.GetDescription() : string.Empty;

                    var countMallInList = result.Where(x => x.MallName == mallName);
                    if (!countMallInList.Any())
                    {
                        var mallItem = args.MallItems.FirstOrDefault(x => x.MallName == mallName);
                        if (mallItem != null)
                            result.Add(mallItem);
                    }
                }
            }
            return result;
        }

        private void UpdateDisplayName(Item item, string value)
        {
            using (new EditContext(item, true, false))
            {
                item.Appearance.DisplayName = value;
            }
        }

        private Item CreateItem(Item parent, string name, string displayName, TemplateItem templateItem, ImportItemsArgs args)
        {
            Item newItem = Utils.SearchChildItem(args, parent, name, templateItem, this, UpdateDisplayName, displayName);

            return newItem;
        }

        private string GetStoreOffers(DataRow dataRow)
        {
            var storeOffer = string.Empty;
            if (dataRow[StoreInputField.GiftCards.ToString()].ToString() == Constants.ChoosedValue)
            {
                storeOffer = Templates.StoreOffer.AcceptsGiftCards.ToString();
            }
            if (dataRow[StoreInputField.DigitalGiftCards.ToString()].ToString() == Constants.ChoosedValue)
            {
                if (string.IsNullOrEmpty(storeOffer))
                    storeOffer = Templates.StoreOffer.DigitalGiftCards.ToString();
                else
                    storeOffer += "|" + Templates.StoreOffer.DigitalGiftCards.ToString();
            }
            if (dataRow[StoreInputField.EarnFrasersPoint.ToString()].ToString() == Constants.ChoosedValue)
            {
                if (string.IsNullOrEmpty(storeOffer))
                    storeOffer = Templates.StoreOffer.EarnFrasersPoints.ToString();
                else
                    storeOffer += "|" + Templates.StoreOffer.EarnFrasersPoints.ToString();
            }
            if (dataRow[StoreInputField.IsHalal.ToString()].ToString() == Constants.ChoosedValue)
            {
                if (string.IsNullOrEmpty(storeOffer))
                    storeOffer = Templates.StoreOffer.HalalCertified.ToString();
                else
                    storeOffer += "|" + Templates.StoreOffer.HalalCertified.ToString();
            }

            return storeOffer;
        }

        private string GetStoreForArticle(string storeName, string mallId, ImportItemsArgs args)
        {
            Item storeItem= null;
            if (!string.IsNullOrEmpty(mallId))
            {
                var storeRootFolderId = args.MallItems.Where(x => x.MallID.ToString() == mallId).Select(x => x.StoreRootID).FirstOrDefault();
                storeItem = GetStoreItem(args, storeRootFolderId, storeName);
            }
            else
            {
                foreach (var mallItem in args.MallItems)
                {
                    storeItem = GetStoreItem(args, mallItem.StoreRootID, storeName);
                    if (storeItem != null)
                        break;
                }
            }

            return storeItem?.ID.ToString();
        }

        private Item GetStoreItem(ImportItemsArgs args, ID storeRootFolderId, string storeName)
        {
            Item storeItem = null;
            var storeRootFolderItem = args.Database.GetItem(storeRootFolderId);
            if (storeRootFolderItem != null)
            {
                storeItem = storeRootFolderItem.GetChildren().FirstOrDefault(x => x.DisplayName.Equals(storeName, StringComparison.OrdinalIgnoreCase) 
                                                                            || x.Name.Equals(storeName, StringComparison.OrdinalIgnoreCase));
            }

            return storeItem;
        }

        private string CreateAndGetBlogCategoryId(string strCategory, ImportItemsArgs args)
        {
            var separator = new[] { args.ImportOptions.MultipleValuesImportSeparator };
            var categoryValues = strCategory != null
                    ? strCategory.Split(separator, StringSplitOptions.RemoveEmptyEntries)
                    : new string[] { };
            StringBuilder result = new StringBuilder();
            foreach (var value in categoryValues)
            {
                if (!value.Equals(BlogCategory.Uncategorised.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    var categoryEnum = Enum.GetValues(typeof(BlogCategory)).Cast<BlogCategory>().FirstOrDefault(x => x.GetDescription() == value.Trim());
                    var categoryName = categoryEnum.ToString();
                    var categoryDisplayName = categoryEnum.GetDescription();

                    var templateItem = args.Database.GetTemplate(Templates.BlogCategories.ID);

                    //get the parent in the specific language
                    Item parent = args.Database.GetItem(Templates.BlogCategories.GlobalRoot);

                    var newItem = CreateItem(parent, categoryName, categoryDisplayName, templateItem, args);

                    if (string.IsNullOrEmpty(result.ToString()))
                        result.Append(newItem.ID.ToString());
                    else result.Append("|" + newItem.ID.ToString());
                }
            }
            return result.ToString();
        }
    }
}