using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Foundation.Import.Pipelines.ImportItems;
using System;
using System.Text.RegularExpressions;

namespace Sitecore.Foundation.Import
{
    public static class Utils
    {
        public static string GetValidItemName(string proposedName)
        {
            var newName = proposedName;
            if (string.IsNullOrWhiteSpace(newName))
            {
                return UnNamedItem;
            }
            newName = ItemUtil.ProposeValidItemName(newName);
            newName = Regex.Replace(newName, @"\s+", " ");
            return newName;
        }

        public static string UnNamedItem
        {
            get { return "Unnamed item"; }
        }

        public static string RemoveHTML(string strHTML)
        {
            return Regex.Replace(strHTML, "<.*?>|&.*?;", string.Empty);
        }

        public static string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, @"[^\w\d]", string.Empty);
        }

        public static string GetFileNoExtension(string fileName)
        {
            int fileExtPos = fileName.LastIndexOf(".");
            if (fileExtPos >= 0)
                fileName = fileName.Substring(0, fileExtPos);

            return fileName;
        }

        public static Item SearchChildItem(ImportItemsArgs args, Item parent, string itemName, TemplateItem templateItem, ImportItemsProcessor processor, Action<Item, string> updateDisplayName = null, string displayName = null)
        {
            // search for the child by name
            Item newItem = parent.GetChildren()[itemName];

            if (newItem != null)
            {
                if (args.ImportOptions.ExistingItemHandling == ExistingItemHandling.AddVersion)
                {
                    args.Statistics.UpdatedItems++;
                    newItem = newItem.Versions.AddVersion();
                    Log.Info(string.Format("Sitecore.Foundation.Import:Creating new version of item {0}", newItem.Paths.ContentPath), processor);
                }
                else if (args.ImportOptions.ExistingItemHandling == ExistingItemHandling.Skip)
                {
                    Log.Info(string.Format("Sitecore.Foundation.Import:Skipping update of item {0}", newItem.Paths.ContentPath), processor);
                    return newItem;
                }
                else if (args.ImportOptions.ExistingItemHandling == ExistingItemHandling.Update)
                {
                    //continue to update current item/version
                    args.Statistics.UpdatedItems++;
                }
            }
            else
            {
                //if not found then create one
                args.Statistics.CreatedItems++;
                newItem = parent.Add(itemName, templateItem);
                Log.Info(string.Format("Sitecore.Foundation.Import:Creating item {0}", newItem.Paths.ContentPath), processor);
                
                //update displayname
                if (updateDisplayName != null && displayName != null)
                {
                    updateDisplayName(newItem, displayName);
                }
            }
            return newItem;
        }
    }
}