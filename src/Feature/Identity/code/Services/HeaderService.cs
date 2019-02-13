using Sitecore.Feature.Identity.Models;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Foundation.DependencyInjection;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Resources.Media;
using System.Linq;
using Sitecore.Foundation.Abstractions.SitecoreContext;
using System;
using Sitecore.Data;

namespace Sitecore.Feature.Identity.Services
{
    [Service(typeof(IHeaderService))]
    public class HeaderService : IHeaderService
    {
        private readonly ISitecoreContext sitecoreContext;
        public HeaderService(ISitecoreContext sitecoreContext)
        {
            this.sitecoreContext = sitecoreContext;
        }

        public MenuItems GetMainMenus()
        {
            return new MenuItems()
            {
                Items = this.GetLinkItems(Templates.HeaderSettings.MainLinks.ID, link => new MenuItem()
                {
                    Url = link.Field(Templates.HeaderSettings.MainLinks.Fields.Link)
                })
            };
        }

        public SocialItems GetSocialIcons()
        {
            return new SocialItems()
            {
                Items = this.GetLinkItems<SocialItem>(Templates.HeaderSettings.SocialLinks.ID,
                link => new SocialItem()
                {
                    Name = link.Name,
                    IconUrl = link.ImageUrl(Templates.HeaderSettings.SocialLinks.Fields.Icon, new MediaUrlOptions
                    {
                        AbsolutePath = true
                    }),
                    MobileIconUrl = link.ImageUrl(Templates.HeaderSettings.SocialLinks.Fields.MobileIcon, new MediaUrlOptions
                    {
                        AbsolutePath = true
                    }),
                    Link = link.LinkFieldUrl(Templates.HeaderSettings.SocialLinks.Fields.Link),
                    Css = link.GetString(Templates.HeaderSettings.SocialLinks.Fields.Css),
                    IconPostText = link.Fields[Templates.HeaderSettings.SocialLinks.Fields.IconPostText]?.Value,
                })
            };
        }

        private List<T> GetLinkItems<T>(ID templateId,  Func<Item, T> generateDataResponse) where T : class
        {
            var items = new List<T>();
            var item = this.sitecoreContext.Database.GetItem(Sitecore.Context.Site.StartPath).Parent;
            item = item.Children.FirstOrDefault(s => s.IsDerived(Templates.HeaderSettings.Settings.ID));

            if (item == null) return items;
            item = item.Children.FirstOrDefault(s => s.IsDerived(Templates.HeaderSettings.ID));

            if (item == null) return items;
            item = item.Children.FirstOrDefault(s => s.IsDerived(templateId));

            if (item == null) return items;
            foreach (Item link in item.Children)
            {
                items.Add(generateDataResponse(link));
            }

            return items;
        }
    }
}