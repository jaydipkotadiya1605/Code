using Sitecore.Foundation.Import.Extensions;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Sitecore.Foundation.Import.Map.CustomItems
{
    public class OutputFieldItem : CustomItem
    {
        public static readonly ID TemplateId = new ID("{317A4F55-F36E-4E6E-A411-85883BFD4496}");

        public OutputFieldItem(Item item) : base(item)
        {
            
        }

        public Item InputField
        {
            get { return ItemExtensions.GetLinkItem(this.InnerItem, "InputField"); }
        }
    }
}