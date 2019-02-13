using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Foundation.Multisite.Model;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Globalization;
using Sitecore.SecurityModel;
using Sitecore.Shell.Applications.ContentEditor;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Sitecore.Foundation.Multisite.Extensions.CustomFields
{
    /// <summary>
    /// Source Path template:  templateFolderId/templateItemId
    ///                    or  templateFolderId/templateFolderId/templateItemId
    /// </summary>
    public class MultiSiteLookupEx : LookupEx
    {
        protected override void DoRender(HtmlTextWriter output)
        {
            Assert.ArgumentNotNull(output, nameof(output));
            Item current = Sitecore.Context.ContentDatabase.Items[base.ItemID];
            var listSiteItems = this.GetListSiteItems(current, this.Source);
            output.Write("<select" + this.GetControlAttributes() + ">");
            output.Write("<option value=\"\"></option>");
            bool itemInGroup = false;

            foreach (var group in listSiteItems)
            {
                if (group.Items == null) continue;
                string itemHeader = this.GetItemHeader(group.Site);
                output.WriteBeginTag("optgroup");
                output.WriteAttribute("label", itemHeader);
                output.Write('>');
                foreach (Item item in group.Items)
                {
                    bool isSeleted = this.IsSelected(item);
                    itemHeader = this.GetItemHeader(item);
                    output.WriteBeginTag("option");
                    output.WriteAttribute("value", this.GetItemValue(item));
                    if (isSeleted)
                    {
                        output.WriteAttribute("selected", "selected");
                        itemInGroup = true;
                    }
                    output.Write('>');
                    output.Write(itemHeader);
                    output.WriteEndTag("option");
                }
                output.WriteEndTag("optgroup");
            }
            bool isItemNotInGroup = !string.IsNullOrEmpty(this.Value) && !itemInGroup;
            if (isItemNotInGroup)
            {
                output.Write("<optgroup label=\"" + Translate.Text("Value not in the selection list.") + "\">");
                string text = HttpUtility.HtmlEncode(this.Value);
                output.Write(string.Concat(new string[]
                {
                    "<option value=\"",
                    text,
                    "\" selected=\"selected\">",
                    text,
                    "</option>"
                }));
                output.Write("</optgroup>");
            }
            output.Write("</select>");
            if (isItemNotInGroup)
            {
                output.Write("<div style=\"color:#999999;padding:2px 0px 0px 0px\">{0}</div>", Translate.Text("The field contains a value that is not in the selection list."));
            }
        }

        private IEnumerable<SiteItemsModel> GetListSiteItems(Item currentItem, string source)
        {
            Assert.ArgumentNotNull(currentItem, nameof(currentItem));
            using (new SecurityDisabler())
            {
                var result = new List<SiteItemsModel>();
                var mainSite = currentItem.GetMainSite() ?? GetMainSiteDefault(currentItem);
                result.Add(GetItemsInSite(mainSite, source));
                foreach (Item mallSite in mainSite?.GetMallSites())
                {
                    result.Add(GetItemsInSite(mallSite, source));
                }
                return result;
            }
        }
        private Item GetMainSiteDefault(Item currentItem)
        {
            var contentRoot = currentItem.Database.Items["sitecore/content"];
            return contentRoot.Children.FirstOrDefault(x => x.IsDerived(Templates.MainSiteSetting.ID));
        }
        private SiteItemsModel GetItemsInSite(Item site, string source)
        {
            if (!source.Contains("/"))
            {
                return null;
            }
            var queryPath = source.Split('/');
            var items = site.Children?.Where(c => c.IsDerived(new ID(queryPath[0])))?.FirstOrDefault()?.Children;
            if (queryPath.Count() == 3) // templateFolderId/templateFolderId/templateItemId
            {
                items = items?.Where(c => c.IsDerived(new ID(queryPath[1])))?.FirstOrDefault()?.Children;
            }
            return new SiteItemsModel
            {
                Site = site,
                Items = items?.Where(x => x.IsDerived(new ID(queryPath.Last()))).ToArray()
            };
        }
    }
}