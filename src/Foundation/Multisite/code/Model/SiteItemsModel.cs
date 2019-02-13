using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Foundation.Multisite.Model
{
    public class SiteItemsModel
    {
        public Item Site { get; set; }
        public Item[] Items { get; set; }
    }
}