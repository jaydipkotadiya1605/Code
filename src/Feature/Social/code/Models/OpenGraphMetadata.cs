using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Social.Models
{
    public class OpenGraphMetadata
    {
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public string ItemUrl { get; set; }
        public string Type { get; set; }
    }
}