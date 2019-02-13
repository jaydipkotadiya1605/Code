using Sitecore.Data;
using Sitecore.Data.Items;
using System.Linq;

namespace Sitecore.Foundation.ItemResolver.Extensions
{
    public static class StringExtensions
    {
        public static ID ItemId(this string id)
        {
            ID result;
            return ID.TryParse(id, out result) ? result : ID.Null;
        }

        public static string ItemName(this string urlPath)
        {
            if (string.IsNullOrWhiteSpace(urlPath))
            {
                return null;
            }

            var segments = urlPath.Split(Constants.UrlSeparator, System.StringSplitOptions.RemoveEmptyEntries);
            var name = segments?.Length != 0 ? segments.Last() : null;

            return name.NormalizeItemName();
        }

        public static string NormalizeItemName(this string itemName)
        {
            if (string.IsNullOrEmpty(itemName))
            {
                return null;
            }

            var replaced = ItemUtil.ProposeValidItemName(itemName).ToLower();
            replaced = replaced.Replace(Constants.Hyphen, Constants.Blank);
            return replaced;
        }
    }
}