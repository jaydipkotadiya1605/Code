using Sitecore.Foundation.Import.Extensions;
using Sitecore.Foundation.Import.Map.CustomItems;
using Sitecore.Data;
using System.Linq;
using System;
using System.Collections.Generic;

namespace Sitecore.Foundation.Import.Map
{
    public static class Factory
    {
        public static ItemImportMap BuildMapInfo(ID mapId)
        {
            var database = Sitecore.Configuration.Factory.GetDatabase("master");
            var mapItem = database.GetItem(mapId);
            var inputColumnsItem =
                mapItem.FirstChildInheritingFrom(InputColumnCollectionItem.TemplateId);

            var mapInfo = new ItemImportMap
            {
                InputFields = inputColumnsItem.Children.Select(c => new InputField {Name = c.Name}).ToList(),
                OutputMaps = mapItem.Children
                    .Where(c => c.InheritsFrom(OutputMapTemplateItem.TemplateId))
                    .Select(om => CreateOutputMap(om, null))
                    .ToList()
            };
            return mapInfo;
        }

        public static ItemImportMap BuildMapInfo(MapItem mapItem, ID templateID, ContentType contentType)
        {

            var mapInfo = new ItemImportMap
            {
                InputFields = mapItem.InputFields.Select(fieldName => new InputField { Name = fieldName }).ToList(),
                OutputMaps = new List<OutputMap>()
            };
            if (contentType == ContentType.Store)
                mapInfo.OutputMaps = SetStoreMap(mapItem, templateID);
            else if (contentType == ContentType.Banner)
                mapInfo.OutputMaps = SetBannerMap(mapItem, templateID);
            else if (contentType == ContentType.Event)
                mapInfo.OutputMaps = SetEventMap(mapItem, templateID);
            else if (contentType == ContentType.Article)
            {
                if (templateID == FrasersContent.Templates.SpecialEvent.ID)
                    mapInfo.OutputMaps = SetSpecialEventMap(mapItem, templateID);
                else
                    mapInfo.OutputMaps = SetArticleMap(mapItem, templateID);
            }
            else if (contentType == ContentType.Blog)
            {
                mapInfo.OutputMaps = SetBlogMap(mapItem, templateID);
            }
            return mapInfo;
        }

        public static MapItem SetMapInputField(string contentType, ID templateID)
        {
            MapItem mapItem = new MapItem();
            if (contentType == ContentType.Store.ToString())
            {
                mapItem.InputFields = Enum.GetNames(typeof(StoreInputField)).ToList();
                mapItem.OutputFields = new DescriptionAttributes<StoreOutputField>().Descriptions.ToList();
            }
            else if (contentType == ContentType.Banner.ToString())
            {
                mapItem.InputFields = Enum.GetNames(typeof(BannerInputField)).ToList();
                mapItem.OutputFields = new DescriptionAttributes<BannerOutputField>().Descriptions.ToList();
            }
            else if (contentType == ContentType.Event.ToString())
            {
                mapItem.InputFields = Enum.GetNames(typeof(EventInputField)).ToList();
                mapItem.OutputFields = new DescriptionAttributes<EventOutputField>().Descriptions.ToList();
            }
            else if (contentType == ContentType.Article.ToString())
            {
                if (templateID == FrasersContent.Templates.SpecialEvent.ID)
                {
                    mapItem.InputFields = Enum.GetNames(typeof(ArticleInputField)).ToList();
                    mapItem.OutputFields = new DescriptionAttributes<SpecialOutputField>().Descriptions.ToList();
                }
                else
                {
                    mapItem.InputFields = Enum.GetNames(typeof(ArticleInputField)).ToList();
                    mapItem.OutputFields = new DescriptionAttributes<ArticleOutputField>().Descriptions.ToList();
                }
            }
            else if (contentType == ContentType.Blog.ToString())
            {
                mapItem.InputFields = new DescriptionAttributes<BlogInputField>().Descriptions.ToList();
                mapItem.OutputFields = new DescriptionAttributes<BlogOutputField>().Descriptions.ToList();
            }
            return mapItem;
        }

        private static OutputMap CreateOutputMap(Data.Items.Item item, OutputMap parentMap)
        {
            var outputMap = new OutputMap();
            outputMap.ParentMap = parentMap;
            var outputMapCustomItem = new OutputMapTemplateItem(item);
            outputMap.TemplateId = outputMapCustomItem.TargetTemplate.ID;
            outputMap.NameInputField = outputMapCustomItem.ItemNameField.Name;
            var fieldsCollection =
                item.Children.FirstOrDefault(c => c.InheritsFrom(OutputFieldCollectionItem.TemplateId));
            if (fieldsCollection != null)
            {
                foreach (var field in fieldsCollection.Children.Where(c => c.InheritsFrom(OutputFieldItem.TemplateId)))
                {
                    var fieldCustomItem = new OutputFieldItem(field);
                    outputMap.Fields.Add(new OutputField
                    {
                        SourceColumn = fieldCustomItem.InputField.Name,
                        TargetFieldName = fieldCustomItem.Name
                    });
                }
            }
            if (!outputMap.Fields.Any())
            {
                outputMap.Fields.Add(new OutputField
                {
                    SourceColumn = outputMap.NameInputField,
                    TargetFieldName = ""
                });
            }
            if (parentMap != null &&
                !outputMap.Fields.Any(f => f.SourceColumn == parentMap.NameInputField))
            {
                outputMap.Fields.Add(new OutputField
                {
                    SourceColumn = parentMap.NameInputField,
                    TargetFieldName = ""
                });
            }

            var childMapItems = item.Children.Where(c => c.InheritsFrom(OutputMapTemplateItem.TemplateId));
            if (childMapItems != null &&
                childMapItems.Any())
            {
                foreach (var childMapItem in childMapItems)
                {
                    outputMap.ChildMaps.Add(CreateOutputMap(childMapItem, outputMap));
                }
            }

            return outputMap;
        }

        private static List<OutputMap> SetStoreMap(MapItem mapItem, ID templateID)
        {
            var outputMaps = new List<OutputMap>();
            var outputMap = new OutputMap();
            foreach (string field in mapItem.OutputFields)
            {
                var outputField = MapStoreField(field);
                outputMap.Fields.Add(outputField);
            }
            
            outputMap.TemplateId = templateID;
            outputMaps.Add(outputMap);
            return outputMaps;
        }

        private static OutputField MapStoreField(string field)
        {
            var outputField = new OutputField();
            if (field == StoreOutputField.Store.GetDescription()
                    || field == StoreOutputField.StoreOffers.GetDescription())
            {
                outputField.SourceColumn = StoreInputField.Store.ToString();
                outputField.TargetFieldName = field;
            }
            else if (field == StoreOutputField.Logo.GetDescription())
            {
                outputField.SourceColumn = StoreInputField.Logo.ToString();
                outputField.TargetFieldName = field;
            }
            else if (field == StoreOutputField.NewDate.GetDescription())
            {
                outputField.SourceColumn = StoreInputField.NewDate.ToString();
                outputField.TargetFieldName = field;
            }
            else if (field == StoreOutputField.Category.GetDescription())
            {
                outputField.SourceColumn = StoreInputField.Category.ToString();
                outputField.TargetFieldName = field;
            }
            else if (field == StoreOutputField.Description.GetDescription())
            {
                outputField.SourceColumn = StoreInputField.Description.ToString();
                outputField.TargetFieldName = field;
            }
            else if (field == StoreOutputField.UnitNo.GetDescription())
            {
                outputField.SourceColumn = StoreInputField.UnitNo.ToString();
                outputField.TargetFieldName = field;
            }
            else if (field == StoreOutputField.Contact.GetDescription()
                || field == StoreOutputField.PhoneNumber.GetDescription())
            {
                outputField.SourceColumn = StoreInputField.Contact.ToString();
                outputField.TargetFieldName = field;
            }
            else if (field == StoreOutputField.OpeningHrs.GetDescription())
            {
                outputField.SourceColumn = StoreInputField.OpeningHrs.ToString();
                outputField.TargetFieldName = field;
            }
            else if (field == StoreOutputField.Brands.GetDescription())
            {
                outputField.SourceColumn = StoreInputField.Brands.ToString();
                outputField.TargetFieldName = field;
            }
            else if (field == StoreOutputField.Keywords.GetDescription())
            {
                outputField.SourceColumn = StoreInputField.Keywords.ToString();
                outputField.TargetFieldName = field;
            }
            else if (field == StoreOutputField.UpcomingDate.GetDescription())
            {
                outputField.SourceColumn = StoreInputField.UpcomingDate.ToString();
                outputField.TargetFieldName = field;
            }
            else if (field == StoreOutputField.ExpiryDate.GetDescription())
            {
                outputField.SourceColumn = StoreInputField.ExpiryDate.ToString();
                outputField.TargetFieldName = field;
            }

            return outputField;
        }

        private static List<OutputMap> SetBannerMap(MapItem mapItem, ID templateID)
        {
            var outputMaps = new List<OutputMap>();
            var outputMap = new OutputMap();
            foreach (string field in mapItem.OutputFields)
            {

                var outputField = new OutputField();
                if (field == BannerOutputField.Title.GetDescription())
                {
                    outputField.SourceColumn = BannerInputField.Title.ToString();
                    outputField.TargetFieldName = field;
                }
                else if (field == BannerOutputField.Image.GetDescription())
                {
                    outputField.SourceColumn = BannerInputField.Image.ToString();
                    outputField.TargetFieldName = field;
                }
                else if (field == BannerOutputField.Summary.GetDescription())
                {
                    outputField.SourceColumn = BannerInputField.Summary.ToString();
                    outputField.TargetFieldName = field;
                }
                else if (field == BannerOutputField.Category.GetDescription())
                {
                    outputField.SourceColumn = BannerInputField.Category.ToString();
                    outputField.TargetFieldName = field;
                }
                else if (field == BannerOutputField.Link.GetDescription())
                {
                    outputField.SourceColumn = BannerInputField.Link.ToString();
                    outputField.TargetFieldName = field;
                }
                else if (field == BannerOutputField.PostDate.GetDescription())
                {
                    outputField.SourceColumn = BannerInputField.PostDate.ToString();
                    outputField.TargetFieldName = field;
                }
                else if (field == BannerOutputField.ExpiryDate.GetDescription())
                {
                    outputField.SourceColumn = BannerInputField.ExpiryDate.ToString();
                    outputField.TargetFieldName = field;
                }
                else if (field == BannerOutputField.ShowInMain.GetDescription())
                {
                    outputField.SourceColumn = BannerInputField.ShowInMain.ToString();
                    outputField.TargetFieldName = field;
                }
                else if (field == BannerOutputField.ShowInMalls.GetDescription())
                {
                    outputField.SourceColumn = BannerInputField.ShowInMalls.ToString();
                    outputField.TargetFieldName = field;
                }
                outputMap.Fields.Add(outputField);
            }

            outputMap.TemplateId = templateID;
            outputMaps.Add(outputMap);
            return outputMaps;
        }

        private static List<OutputMap> SetEventMap(MapItem mapItem, ID templateID)
        {
            var outputMaps = new List<OutputMap>();
            var outputMap = new OutputMap();
            foreach (string field in mapItem.OutputFields)
            {

                var outputField = new OutputField();
                if (field == EventOutputField.Title.GetDescription())
                {
                    outputField.SourceColumn = BannerInputField.Title.ToString();
                    outputField.TargetFieldName = field;
                }
                else if (field == EventOutputField.PostDate.GetDescription())
                {
                    outputField.SourceColumn = EventInputField.PostDate.ToString();
                    outputField.TargetFieldName = field;
                }
                else if (field == EventOutputField.Store.GetDescription()
                    || field == EventOutputField.AllStore.GetDescription())
                {
                    outputField.SourceColumn = EventInputField.Store.ToString();
                    outputField.TargetFieldName = field;
                }
                else if (field == EventOutputField.Keywords.GetDescription())
                {
                    outputField.SourceColumn = EventInputField.Keywords.ToString();
                    outputField.TargetFieldName = field;
                }
                else if (field == EventOutputField.StartDate.GetDescription())
                {
                    outputField.SourceColumn = EventInputField.Start_Date.ToString();
                    outputField.TargetFieldName = field;
                }
                else if (field == EventOutputField.EndDate.GetDescription())
                {
                    outputField.SourceColumn = EventInputField.End_Date.ToString();
                    outputField.TargetFieldName = field;
                }
                else if (field == EventOutputField.Image.GetDescription())
                {
                    outputField.SourceColumn = EventInputField.Image.ToString();
                    outputField.TargetFieldName = field;
                }
                else if (field == EventOutputField.Summary.GetDescription())
                {
                    outputField.SourceColumn = EventInputField.Summary.ToString();
                    outputField.TargetFieldName = field;
                }
                else if (field == EventOutputField.EventType.GetDescription())
                {
                    outputField.SourceColumn = EventInputField.EventType.ToString();
                    outputField.TargetFieldName = field;
                }
                else if (field == EventOutputField.Thumbnail.GetDescription())
                {
                    outputField.SourceColumn = EventInputField.Thumbnail.ToString();
                    outputField.TargetFieldName = field;
                }
                else if (field == EventOutputField.Description.GetDescription())
                {
                    outputField.SourceColumn = EventInputField.Description.ToString();
                    outputField.TargetFieldName = field;
                }
                else if (field == EventOutputField.ShowInMalls.GetDescription())
                {
                    outputField.SourceColumn = EventInputField.ShowInMalls.ToString();
                    outputField.TargetFieldName = field;
                }
                outputMap.Fields.Add(outputField);
            }

            outputMap.TemplateId = templateID;
            outputMaps.Add(outputMap);
            return outputMaps;
        }

        private static List<OutputMap> SetArticleMap(MapItem mapItem, ID templateID)
        {
            var outputMaps = new List<OutputMap>();
            var outputMap = new OutputMap();
            foreach (string field in mapItem.OutputFields)
            {

                var outputField = new OutputField();
                if (field == ArticleOutputField.Title.GetDescription())
                {
                    outputField.SourceColumn = ArticleInputField.Title.ToString();
                    outputField.TargetFieldName = field;
                }
                else if (field == ArticleOutputField.Thumbnail.GetDescription())
                {
                    outputField.SourceColumn = ArticleInputField.Thumbnail.ToString();
                    outputField.TargetFieldName = field;
                }
                else if (field == ArticleOutputField.Banner.GetDescription())
                {
                    outputField.SourceColumn = ArticleInputField.Banner.ToString();
                    outputField.TargetFieldName = field;
                }
                else if (field == ArticleOutputField.PostDate.GetDescription())
                {
                    outputField.SourceColumn = ArticleInputField.PostDate.ToString();
                    outputField.TargetFieldName = field;
                }
                else if (field == ArticleOutputField.ExpiryDate.GetDescription())
                {
                    outputField.SourceColumn = ArticleInputField.ExpiryDate.ToString();
                    outputField.TargetFieldName = field;
                }
                else if (field == ArticleOutputField.Summary.GetDescription())
                {
                    outputField.SourceColumn = ArticleInputField.Summary.ToString();
                    outputField.TargetFieldName = field;
                }
                else if (field == ArticleOutputField.Description.GetDescription())
                {
                    outputField.SourceColumn = ArticleInputField.Description.ToString();
                    outputField.TargetFieldName = field;
                }
                else if (field == ArticleOutputField.Store.GetDescription())
                {
                    outputField.SourceColumn = ArticleInputField.Store.ToString();
                    outputField.TargetFieldName = field;
                }
                else if (field == ArticleOutputField.StartDate.GetDescription())
                {
                    outputField.SourceColumn = ArticleInputField.StartDate.ToString();
                    outputField.TargetFieldName = field;
                }
                else if (field == ArticleOutputField.EndDate.GetDescription())
                {
                    outputField.SourceColumn = ArticleInputField.EndDate.ToString();
                    outputField.TargetFieldName = field;
                }
                else if (field == ArticleOutputField.ShowInMalls.GetDescription())
                {
                    outputField.SourceColumn = ArticleInputField.ShowInMalls.ToString();
                    outputField.TargetFieldName = field;
                }
                else if (field == ArticleOutputField.Category.GetDescription())
                {
                    outputField.SourceColumn = ArticleInputField.Category.ToString();
                    outputField.TargetFieldName = field;
                }
                outputMap.Fields.Add(outputField);
            }

            outputMap.TemplateId = templateID;
            outputMaps.Add(outputMap);
            return outputMaps;
        }

        private static List<OutputMap> SetSpecialEventMap(MapItem mapItem, ID templateID)
        {
            var outputMaps = new List<OutputMap>();
            var outputMap = new OutputMap();
            foreach (string field in mapItem.OutputFields)
            {
                OutputField outputField = null;
                if (field == SpecialOutputField.Title.GetDescription())
                {
                    outputField = new OutputField(){SourceColumn = ArticleInputField.Title.ToString(), TargetFieldName = field};
                }
                else if (field == SpecialOutputField.Thumbnail.GetDescription())
                {
                    outputField = new OutputField(){ SourceColumn = ArticleInputField.Thumbnail.ToString(), TargetFieldName = field };
                }
                else if (field == SpecialOutputField.Image.GetDescription())
                {
                    outputField = new OutputField(){SourceColumn = ArticleInputField.Banner.ToString(), TargetFieldName = field};
                }
                else if (field == SpecialOutputField.PostDate.GetDescription())
                {
                    outputField = new OutputField() {SourceColumn = ArticleInputField.PostDate.ToString(), TargetFieldName = field};
                }
                else if (field == SpecialOutputField.ExpiryDate.GetDescription())
                {
                    outputField = new OutputField(){SourceColumn = ArticleInputField.ExpiryDate.ToString(), TargetFieldName = field};
                }
                else if (field == SpecialOutputField.Summary.GetDescription())
                {
                    outputField = new OutputField() { SourceColumn = ArticleInputField.Summary.ToString(), TargetFieldName = field};
                }
                else if (field == SpecialOutputField.Description.GetDescription())
                {
                    outputField = new OutputField() { SourceColumn = ArticleInputField.Description.ToString(), TargetFieldName = field };
                }
                else if (field == SpecialOutputField.Store.GetDescription())
                {
                    outputField = new OutputField() { SourceColumn = ArticleInputField.Store.ToString(), TargetFieldName = field};
                }
                else if (field == SpecialOutputField.StartDate.GetDescription())
                {
                    outputField = new OutputField(){SourceColumn = ArticleInputField.StartDate.ToString(), TargetFieldName = field };
                }
                else if (field == SpecialOutputField.EndDate.GetDescription())
                {
                    outputField = new OutputField() { SourceColumn = ArticleInputField.EndDate.ToString(), TargetFieldName = field };
                }
                else if (field == SpecialOutputField.ShowInMalls.GetDescription()) {
                    outputField = new OutputField()
                    {
                        SourceColumn = ArticleInputField.ShowInMalls.ToString(),
                        TargetFieldName = field
                    };
                }
                if(outputField != null )
                    outputMap.Fields.Add(outputField);
            }

            outputMap.TemplateId = templateID;
            outputMaps.Add(outputMap);
            return outputMaps;
        }

        private static List<OutputMap> SetBlogMap(MapItem mapItem, ID templateID)
        {
            var outputMaps = new List<OutputMap>();
            var outputMap = new OutputMap();
            foreach (string field in mapItem.OutputFields)
            {

                var outputField = new OutputField();
                if (field == BlogOutputField.Category.GetDescription())
                {
                    outputField.SourceColumn = BlogInputField.Category.GetDescription();
                    outputField.TargetFieldName = field;
                }
                else if (field == BlogOutputField.Title.GetDescription())
                {
                    outputField.SourceColumn = BlogInputField.Title.GetDescription();
                    outputField.TargetFieldName = field;
                }
                else if (field == BlogOutputField.Thumbnail.GetDescription())
                {
                    outputField.SourceColumn = BlogInputField.Thumbnail.GetDescription();
                    outputField.TargetFieldName = field;
                }
                else if (field == BlogOutputField.Banner.GetDescription())
                {
                    outputField.SourceColumn = BlogInputField.Banner.GetDescription();
                    outputField.TargetFieldName = field;
                }
                else if (field == BlogOutputField.Author.GetDescription())
                {
                    outputField.SourceColumn = BlogInputField.AuthorName.GetDescription();
                    outputField.TargetFieldName = field;
                }
                else if (field == BlogOutputField.PostDate.GetDescription())
                {
                    outputField.SourceColumn = BlogInputField.PostDate.GetDescription();
                    outputField.TargetFieldName = field;
                }
                else if (field == BlogOutputField.ExpiryDate.GetDescription())
                {
                    outputField.SourceColumn = BlogInputField.ExpiryDate.GetDescription();
                    outputField.TargetFieldName = field;
                }
                else if (field == BlogOutputField.Summary.GetDescription())
                {
                    outputField.SourceColumn = BlogInputField.Summary.GetDescription();
                    outputField.TargetFieldName = field;
                }
                else if (field == BlogOutputField.Body.GetDescription())
                {
                    outputField.SourceColumn = BlogInputField.Body.ToString();
                    outputField.TargetFieldName = field;
                }
                outputMap.Fields.Add(outputField);
            }

            outputMap.TemplateId = templateID;
            outputMaps.Add(outputMap);
            return outputMaps;
        }

    }
}