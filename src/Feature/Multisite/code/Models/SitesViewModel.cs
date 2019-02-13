using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Multisite.Models
{
    public class SitesViewModel
    {
        public IList<CommercialMenuSite> CommercialMenuSites { get; set; }
        public IList<MallMenuSite> MallMenuSites { get; set; }
        public int PageSizeMallMenuSite { get; set; }
        public int RepeatLoopMallSites { get; set; }
        public int PageSizeCommerceSite { get; set; }
        public int RepeatLoopCommerceSite { get; set; }
        public string GroupMenuName { get; set; }
    }
}