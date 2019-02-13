namespace Sitecore.Feature.Navigation.Repositories
{
    using Sitecore.ContentSearch.Utilities;
    using Sitecore.Data.Items;
    using Sitecore.Feature.Navigation.Models;
    using Sitecore.Foundation.Abstractions.SitecoreContext;
    using Sitecore.Foundation.DependencyInjection;
    using Sitecore.Foundation.SitecoreExtensions.Extensions;
    using Sitecore.Foundation.ItemResolver.Extensions;
    using FrasersContent = Sitecore.Foundation.FrasersContent;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Sitecore.Data;

    [Service(typeof(INavigationRepository))]
    public class NavigationRepository : INavigationRepository
    {
        public ISitecoreContext SitecoreContext { get; }
        public Item NavigationRoot
        {
            get
            {
                var root = this.GetNavigationRoot(this.SitecoreContext.Item);
                if (root == null)
                {
                    throw new InvalidOperationException($"Cannot determine navigation root from '{this.SitecoreContext.Item.Paths.FullPath}'");
                }
                return root;
            }
        }

        public NavigationRepository(ISitecoreContext SitecoreContext)
        {
            this.SitecoreContext = SitecoreContext;
        }
        public Item GetNavigationRoot(Item contextItem)
        {
            return contextItem.GetAncestorOrSelfOfTemplate(Templates.NavigationRoot.ID) ?? Context.Site.GetContextItem(Templates.NavigationRoot.ID);
        }

        public NavigationItems GetMainMenu()
        {
            var navItems = this.GetChildNavigationItems(this.NavigationRoot, 0, 1);
            this.ModifyActivatedOfWildCardItem(navItems);
            this.AddRootToMainMenu(navItems);
            return navItems;
        }

        public NavigationItems GetBreadcrumb()
        {
            var items = new NavigationItems
            {
                Items = this.GetNavigationHierarchy().Reverse().ToList()
            };

            for (var i = 0; i < items.Items.Count - 1; i++)
            {
                items.Items[i].Level = i;
                items.Items[i].IsActive = i == items.Items.Count - 1;
            }

            return items;
        }

        public NavigationItems GetTabMenu(bool isGetSubTab)
        {
            var items = new NavigationItems();
            var parentItem = GetParentItemTabMenuAndSubTab(isGetSubTab);
            if (parentItem != null)
            {
                items = this.GetChildNavigationItems(parentItem, 0, 0, isGetSubTab, !isGetSubTab);
            }

            if (items != null && items.Items?.Count > 0)
            {
                var currentItem = items.Items.FirstOrDefault(x => x.IsCurrentItem);
                currentItem = currentItem ?? items.Items.FirstOrDefault(x => x.IsActive);
                if (currentItem == null)
                {
                    items.Items[0].IsCurrentItem = true;
                    items.Items[0].IsActive = true;
                }
            }
            return items;
        }

        public NavigationItems GetTabFilter(string categoryName, string datasource, string mallId)
        {
            var items = new NavigationItems();
            var contextItem = this.SitecoreContext.Item;
            mallId = !string.IsNullOrWhiteSpace(mallId) ? mallId : contextItem.GetRootSiteId();
            if (contextItem != null)
            {
                List<Item> result = GetItemsFromDatasource(datasource, mallId);
                if (result != null && result.Count > 0)
                {
                    items.Items = result.Select(item => new NavigationItem()
                    {
                        Item = item,
                        OrderNumber = item.IsDerived(FrasersContent.Templates.TabCategory.ID) ? item.GetString(FrasersContent.Templates.TabCategory.Fields.OrderNumber) : string.Empty,
                        IsCurrentItem = item.IsDerived(FrasersContent.Templates.TabCategory.ID) && (item.Fields[FrasersContent.Templates.TabCategory.Fields.Value].Value == categoryName)
                    }).OrderBy(x => x.OrderNumber).ToList();
                }

                if (items.Items != null && items.Items.Count > 0)
                {
                    var currentItem = items.Items.FirstOrDefault(x => x.IsCurrentItem);
                    if (currentItem == null)
                    {
                        items.Items[0].IsCurrentItem = true;
                        items.Items[0].IsActive = true;
                    }
                }
            }
            return items;
        }

        public NavigationItems GetHorizontalTabPages()
        {
            var items = new NavigationItems()
            {
                Items = new List<NavigationItem>()
            };
            var parentItem = GetParentItemTabPage();
            if (parentItem != null)
            {
                items = this.GetChildNavigationItems(parentItem, 0, 0, false, false, true);
            }

            if (items != null && items.Items.Count > 0)
            {
                var currentItem = items.Items.FirstOrDefault(x => x.IsCurrentItem);
                currentItem = currentItem ?? items.Items.FirstOrDefault(x => x.IsActive);
                if (currentItem == null)
                {
                    items.Items[0].IsCurrentItem = true;
                    items.Items[0].IsActive = true;
                }
            }
            return items;
        }

        private List<Item> GetItemsFromDatasource(string datasource, string mallId)
        {
            List<Item> result = new List<Item>();
            if (ID.IsID(datasource))
            {
                var item = SitecoreContext.Database.GetItem(datasource);
                result = item?.GetChildren().Where(x => !x.HideInSite(mallId)).ToList();
            }

            return result;
        }

        private Item GetParentItemTabMenuAndSubTab(bool isGetSubTab)
        {
            var currentItem = this.SitecoreContext.Item;
            if (currentItem.IsDerived(Foundation.FrasersContent.Templates.Navigable.ID))
            {
                var isTab = currentItem.Fields[Foundation.FrasersContent.Templates.Navigable.Fields.IsTab].IsChecked();
                var isSubTab = currentItem.Fields[Foundation.FrasersContent.Templates.Navigable.Fields.IsSubTab].IsChecked();

                if (!isGetSubTab)
                    return GetParentForTabMenu(isTab, isSubTab, currentItem);
                else
                    return GetParentForSubTabMenu(isTab, isSubTab, currentItem);
            }
            return null;
        }

        private Item GetParentItemTabPage()
        {
            return GetParentForTabMenu(false, false, Sitecore.Context.Site.GetStartItem());
        }

        private Item GetParentForTabMenu(bool isTab, bool isSubTab, Item currentItem)
        {
            if (!isTab && !isSubTab)
            {
                return currentItem;
            }
            else if (isTab && !isSubTab)
            {
                return currentItem.Parent;
            }
            else
            {
                var tabItem = currentItem.Parent;
                return tabItem?.Parent;
            }
        }

        private Item GetParentForSubTabMenu(bool isTab, bool isSubTab, Item currentItem)
        {
            if (!isTab && !isSubTab)
            {
                if (currentItem.HasChildren)
                    return currentItem.Children[0];
                else return null;
            }
            else if (isTab && !isSubTab)
            {
                return currentItem;
            }
            else
            {
                return currentItem.Parent;
            }
        }

        private void AddRootToMainMenu(NavigationItems navItems)
        {
            if (!this.IncludeInNavigation(this.NavigationRoot))
            {
                return;
            }

            var navigationItem = this.CreateNavigationItem(this.NavigationRoot, 0, 0);

            //Root navigation item is only active when we are actually on the root item
            navigationItem.IsActive = this.SitecoreContext.Item.ID == this.NavigationRoot.ID;
            navItems?.Items?.Insert(0, navigationItem);
        }

        private bool IncludeInNavigation(Item item, bool forceShowInMenu = false)
        {
            return item.IsDerived(Foundation.FrasersContent.Templates.Navigable.ID) && (forceShowInMenu || item.Fields[Foundation.FrasersContent.Templates.Navigable.Fields.ShowInNavigation].IsChecked());
        }


        private NavigationItem CreateNavigationItem(Item item, int level, int maxLevel = -1)
        {
            if (item != null)
            {
                var targetItem = item.IsDerived(FrasersContent.Templates.Link.ID)
                    ? item.TargetItem(FrasersContent.Templates.Link.Fields.Link)
                    : item;

                return new NavigationItem
                {
                    Item = item,
                    Url = item.IsDerived(FrasersContent.Templates.Link.ID) ? item.LinkFieldUrl(FrasersContent.Templates.Link.Fields.Link) : item.Url(),
                    IsActive = (targetItem != null && this.IsItemActive(targetItem)) || this.IsItemActive(item),
                    IsCurrentItem = IsCurrentItem(item),
                    Children = this.GetChildNavigationItems(item, level + 1, maxLevel),
                    ShowChildren = !item.IsDerived(FrasersContent.Templates.Navigable.ID) || item.Fields[FrasersContent.Templates.Navigable.Fields.ShowChildren].IsChecked()
                };
            }
            return null;
        }


        private NavigationItems GetChildNavigationItems(Item parentItem, int level, int maxLevel, bool isGetSubTabMenu = false, bool isGetTabMenu = false, bool isGetTabPage = false)
        {
            if (level > maxLevel || !parentItem.HasChildren)
            {
                return null;
            }
            var childItems = parentItem.Children.Where(item => this.IncludeInNavigation(item)).Select(i => this.CreateNavigationItem(i, level, maxLevel));
            if (isGetSubTabMenu)
                childItems = childItems.Where(x => x.Item.Fields[Foundation.FrasersContent.Templates.Navigable.Fields.IsSubTab].IsChecked());
            if (isGetTabMenu)
                childItems = childItems.Where(x => x.Item.Fields[Foundation.FrasersContent.Templates.Navigable.Fields.IsTab].IsChecked());
            if (isGetTabPage)
            {
                childItems = parentItem.Children.Where(item => item.IsDerived(Foundation.FrasersContent.Templates.Navigable.ID))
                                                .Select(i => this.CreateNavigationItem(i, level, maxLevel));
                childItems = childItems.Where(x => x.Item.Fields[Foundation.FrasersContent.Templates.Navigable.Fields.IsTabPage].IsChecked());
            }
            return new NavigationItems
            {
                Items = childItems.Where(x => x != null).ToList()
            };
        }

        private bool IsItemActive(Item item)
        {
            return this.SitecoreContext.Item.ID == item.ID || this.SitecoreContext.Item.Axes.GetAncestors().Any(a => a.ID == item.ID);
        }
        private bool IsCurrentItem(Item item) => item.ID.Equals(SitecoreContext.Item.ID);

        private IEnumerable<NavigationItem> GetNavigationHierarchy()
        {
            var item = this.SitecoreContext.Item;
            while (item != null)
            {
                if (this.IncludeInNavigation(item, true))
                {
                    yield return this.CreateNavigationItem(item, 0);
                }

                item = item.GetParent();
            }
        }
        private void ModifyActivatedOfWildCardItem(NavigationItems navigationItems)
        {
            navigationItems.Items = navigationItems.Items.Select(x => ModifyWildCardIsActive(x)).ToList();
        }
        private NavigationItem ModifyWildCardIsActive(NavigationItem navigationItem)
        {
            if (navigationItem.Item.HasWildCard())
            {
                navigationItem.IsActive =
                    navigationItem.Item.ID == this.SitecoreContext.Item.ID ||
                    navigationItem.Item.ID == this.SitecoreContext.Item.GetParent().ID;
                return navigationItem;
            }
            return navigationItem;
        }
    }
}