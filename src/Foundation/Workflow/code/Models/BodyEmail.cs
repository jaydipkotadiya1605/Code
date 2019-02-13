using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Foundation.Workflow.Models
{
    public class BodyEmail
    {
        public string ReceiverName { get; set; }
        public string Comment { get; set; }
        public string ItemName { get; set; }
        public string ItemUrl { get; set; }

    }
}