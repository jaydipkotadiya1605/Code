namespace Sitecore.Foundation.SitecoreExtensions.Extensions
{
    using Sitecore.Data;
    using Sitecore.Data.Fields;
    using Sitecore.Data.Items;
    using Sitecore.Data.Managers;
    using Sitecore.Diagnostics;
    using Sitecore.Foundation.SitecoreExtensions.Services;
    using Sitecore.Globalization;
    using Sitecore.Links;
    using Sitecore.Resources.Media;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Sitecore.Foundation.SitecoreExtensions.Models;
    using Sitecore.ContentSearch.Utilities;

    public static class ItemExtensions
    {
        public static string Url(this Item item, UrlOptions options = null)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            if (options != null)
                return LinkManager.GetItemUrl(item, options);
            return !item.Paths.IsMediaItem ? LinkManager.GetItemUrl(item) : MediaManager.GetMediaUrl(item);
        }

        public static string ImageUrl(this Item item, ID imageFieldId, MediaUrlOptions options = null)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            var imageField = (ImageField)item.Fields[imageFieldId];
            return imageField?.MediaItem == null ? string.Empty : imageField.ImageUrl(options);
        }

        public static string ImageUrl(this MediaItem mediaItem, int width, int height)
        {
            if (mediaItem == null)
                throw new ArgumentNullException(nameof(mediaItem));

            var options = new MediaUrlOptions { Height = height, Width = width };
            var url = MediaManager.GetMediaUrl(mediaItem, options);
            var cleanUrl = StringUtil.EnsurePrefix('/', url);
            var hashedUrl = HashingUtils.ProtectAssetUrl(cleanUrl);

            return hashedUrl;
        }

        public static string ImageUrl(this MediaItem mediaItem, MediaUrlOptions options = null)
        {
            if (mediaItem == null)
                throw new ArgumentNullException(nameof(mediaItem));
            var url = MediaManager.GetMediaUrl(mediaItem, options ?? MediaUrlOptions.Empty);
            var cleanUrl = StringUtil.EnsurePrefix('/', url);
            var hashedUrl = HashingUtils.ProtectAssetUrl(cleanUrl);

            return hashedUrl;
        }


        public static Item TargetItem(this Item item, ID linkFieldId)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            if (item.Fields[linkFieldId] == null || !item.Fields[linkFieldId].HasValue)
                return null;
            return ((LinkField)item.Fields[linkFieldId]).TargetItem ?? ((ReferenceField)item.Fields[linkFieldId]).TargetItem;
        }

        public static string MediaUrl(this Item item, ID mediaFieldId, MediaUrlOptions options = null)
        {
            var targetItem = item.TargetItem(mediaFieldId);
            return targetItem == null ? string.Empty : (MediaManager.GetMediaUrl(targetItem) ?? string.Empty);
        }


        public static bool IsImage(this Item item)
        {
            return new MediaItem(item).MimeType.StartsWith("image/", StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool IsVideo(this Item item)
        {
            return new MediaItem(item).MimeType.StartsWith("video/", StringComparison.InvariantCultureIgnoreCase);
        }

        public static Item GetAncestorOrSelfOfTemplate(this Item item, ID templateID)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            return item.IsDerived(templateID) ? item : item.Axes.GetAncestors().LastOrDefault(i => i.IsDerived(templateID));
        }

        public static IList<Item> GetAncestorsAndSelfOfTemplate(this Item item, ID templateID)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            var returnValue = new List<Item>();
            if (item.IsDerived(templateID))
                returnValue.Add(item);

            returnValue.AddRange(item.Axes.GetAncestors().Reverse().Where(i => i.IsDerived(templateID)));
            return returnValue;
        }

        public static string LinkFieldUrl(this Item item, ID fieldID)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            if (ID.IsNullOrEmpty(fieldID))
            {
                throw new ArgumentNullException(nameof(fieldID));
            }
            var field = item.Fields[fieldID];
            if (field == null || !(FieldTypeManager.GetField(field) is LinkField))
            {
                return string.Empty;
            }
            else
            {
                return GetFieldUrl(field);
            }
        }

        private static string GetFieldUrl(Field field)
        {
            LinkField linkField = (LinkField)field;
            switch (linkField.LinkType.ToLower())
            {
                case "internal":
                    // Use LinkMananger for internal links, if link is not empty
                    return linkField.TargetItem != null ? Sitecore.Links.LinkManager.GetItemUrl(linkField.TargetItem) : string.Empty;
                case "media":
                    // Use MediaManager for media links, if link is not empty
                    return linkField.TargetItem != null ? Sitecore.Resources.Media.MediaManager.GetMediaUrl(linkField.TargetItem) : string.Empty;
                case "external":
                    // Just return external links
                    return linkField.Url;
                case "anchor":
                    // Prefix anchor link with # if link if not empty
                    return !string.IsNullOrEmpty(linkField.Anchor) ? "#" + linkField.Anchor : string.Empty;
                case "mailto":
                    // Just return mailto link
                    return linkField.Url;
                case "javascript":
                    // Just return javascript
                    return linkField.Url;
                default:
                    // Just please the compiler, this
                    // condition will never be met
                    return linkField.Url;
            }
        }

        public static string LinkFieldTarget(this Item item, ID fieldID)
        {
            return item.LinkFieldOptions(fieldID, LinkFieldOption.Target);
        }

        public static string LinkFieldOptions(this Item item, ID fieldID, LinkFieldOption option)
        {
            XmlField field = item.Fields[fieldID];
            switch (option)
            {
                case LinkFieldOption.Text:
                    return field?.GetAttribute("text");
                case LinkFieldOption.LinkType:
                    return field?.GetAttribute("linktype");
                case LinkFieldOption.Class:
                    return field?.GetAttribute("class");
                case LinkFieldOption.Alt:
                    return field?.GetAttribute("title");
                case LinkFieldOption.Target:
                    return field?.GetAttribute("target");
                case LinkFieldOption.QueryString:
                    return field?.GetAttribute("querystring");
                default:
                    throw new ArgumentOutOfRangeException(nameof(option), option, null);
            }
        }

        public static bool HasLayout(this Item item)
        {
            return item?.Visualization?.Layout != null;
        }


        public static bool IsDerived(this Item item, ID templateId)
        {
            if (item == null)
                return false;

            return !templateId.IsNull && item.IsDerived(item.Database.Templates[templateId]);
        }

        private static bool IsDerived(this Item item, Item templateItem)
        {
            if (item == null)
                return false;

            if (templateItem == null)
                return false;

            var itemTemplate = TemplateManager.GetTemplate(item);
            return itemTemplate != null && (itemTemplate.ID == templateItem.ID || itemTemplate.DescendsFrom(templateItem.ID));
        }

        public static bool FieldHasValue(this Item item, ID fieldID)
        {
            return item.Fields[fieldID] != null && !string.IsNullOrWhiteSpace(item.Fields[fieldID].Value);
        }

        public static int? GetInteger(this Item item, ID fieldId)
        {
            int result;
            return !int.TryParse(item.Fields[fieldId].Value, out result) ? new int?() : result;
        }

        public static IEnumerable<Item> GetMultiListValueItems(this Item item, ID fieldId)
        {
            return new MultilistField(item.Fields[fieldId]).GetItems();
        }

        public static bool HasContextLanguage(this Item item)
        {
            var latestVersion = item.Versions.GetLatestVersion();
            return latestVersion?.Versions.Count > 0;
        }

        public static HtmlString Field(this Item item, ID fieldId)
        {
            Assert.IsNotNull(item, "Item cannot be null");
            Assert.IsNotNull(fieldId, "FieldId cannot be null");
            return new HtmlString(FieldRendererService.RenderField(item, fieldId));
        }

        public static bool HasVersion(this Item item)
        {
            return HasVersion(item, Context.Language);
        }

        public static bool HasVersion(this Item item, Language language)
        {
            Assert.IsNotNull(item, "Item cannot be null");
            Assert.IsNotNull(language, "Language cannot be null");
            Item itemInLanguage = Context.Database.GetItem(item.ID, language);
            return itemInLanguage.Versions.Count > 0;
        }

        public static DateTime GetDateTime(this Item item, ID fieldId)
        {
            if (!item.FieldHasValue(fieldId))
            {
                return DateTime.MinValue;
            }

            return DateUtil.IsoDateToDateTime(item.Fields[fieldId].Value);
        }
        public static string GetDateTimeDefaultFormat(this Item item, ID fieldId)
        {
            const string defaultFormat = "dd MMM yyyy";
            if (!item.FieldHasValue(fieldId))
            {
                return DateTime.MinValue.ToString(defaultFormat);
            }
            return DateUtil.IsoDateToDateTime(item.Fields[fieldId].Value).ToString(defaultFormat);
        }
        public static string GetString(this Item item, ID fieldId)
        {
            if (item.FieldHasValue(fieldId))
            {
                return item.Fields[fieldId].Value;
            }

            return string.Empty;
        }
        public static Item GetDroplinkItem(this Item item, ID fieldId)
        {
            Field field = item.Fields[fieldId];
            if (field == null || string.IsNullOrEmpty(field.Value))
            {
                return null;
            }
            return item.Database.GetItem(new ID(field.Value));
        }

        public static bool GetBoolFieldValue(this Item item, ID fieldID)
        {
            try
            {
                CheckboxField fieldValue = (CheckboxField)item.Fields[fieldID];
                if (fieldValue != null)
                    return fieldValue.Checked;
                return false;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                return false;
            }
        }

        public static ImageItem BuildImageItem(this Item item, ID fieldId, MediaUrlOptions options = null)
        {
            var imageField = (ImageField)item.Fields[fieldId];

            var imageUrl = imageField?.MediaItem == null ? string.Empty : imageField.ImageUrl(options);
            var imageAlt = imageField?.Alt ?? string.Empty;

            var imageItem = new ImageItem()
            {
                Item = item,
                Src = imageUrl,
                Alt = imageAlt,
                Width = imageField?.Width,
                Heigth = imageField?.Height
            };

            return imageItem;
        }

        public static bool HideInSite(this Item item, string mallId)
        {
            bool hide = false;
            if (string.IsNullOrWhiteSpace(mallId) || item.Fields[SitecoreExtensions.Constants.HideInSitesID] == null)
                return hide;

            List<string> hideInSites = ((MultilistField)item.Fields[SitecoreExtensions.Constants.HideInSitesID])?.GetItems()
                                            ?.Select(x => IdHelper.NormalizeGuid(x.ID))?.ToList();
            if (hideInSites.Contains(mallId))
            {
                hide = true;
            }

            return hide;
        }

        public static string GetRootSiteId(this Item item)
        {
            string siteId = string.Empty;
            bool siteFound = false;
            Item siteItem = null;
            if (item != null)
            {
                if (item.TemplateID == SitecoreExtensions.Constants.MainWebsiteID ||
                    item.TemplateID == SitecoreExtensions.Constants.MallWebsiteID ||
                    item.TemplateID == SitecoreExtensions.Constants.CommercialWebsiteID
                   )
                {
                    siteItem = item;
                    siteFound = true;
                }
            }

            if (!siteFound && item != null)
            {
                Item parent = item.Axes.GetAncestors().LastOrDefault(x =>
                                                                     x.TemplateID == SitecoreExtensions.Constants.MainWebsiteID ||
                                                                     x.TemplateID == SitecoreExtensions.Constants.MallWebsiteID ||
                                                                     x.TemplateID == SitecoreExtensions.Constants.CommercialWebsiteID);
                if (parent != null)
                {
                    siteItem = parent;
                }
            }

            if (siteItem != null)
            {
                siteId = IdHelper.NormalizeGuid(siteItem.ID);
            }

            return siteId;
        }
    }

    public enum LinkFieldOption
    {
        Text,
        LinkType,
        Class,
        Alt,
        Target,
        QueryString
    }
}