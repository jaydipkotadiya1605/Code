﻿using Sitecore.Data.Items;

namespace Sitecore.Feature.Navigation.Models
{
    public class NavigationItem
    {
        public Item Item { get; set; }
        public string Url { get; set; }
        public string OrderNumber { get; set; }
        public bool IsActive { get; set; }
        public bool IsCurrentItem { get; set; }
        public int Level { get; set; }
        public NavigationItems Children { get; set; }
        public string Target { get; set; }
        public bool ShowChildren { get; set; }
    }
}