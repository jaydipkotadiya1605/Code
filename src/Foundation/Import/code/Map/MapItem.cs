using System.Collections.Generic;

namespace Sitecore.Foundation.Import.Map
{
    public class MapItem
    {
        public List<string> InputFields { get; set; }
        public List<string> OutputFields { get; set; }

        public MapItem()
        {
            InputFields = new List<string>();
            OutputFields = new List<string>();
        }
        
    }
}