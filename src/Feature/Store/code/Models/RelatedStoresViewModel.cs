using System.Collections.Generic;

namespace Sitecore.Feature.Store.Models
{
    public class RelatedStoresViewModel
    {
        public IEnumerable<StoreViewModel> Stores { get; set; }
    }
}