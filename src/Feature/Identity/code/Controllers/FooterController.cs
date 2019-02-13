namespace Sitecore.Feature.Identity.Controllers
{
    using Sitecore.Data.Items;
    using Sitecore.Feature.Identity.Services;
    using System.Web.Mvc;
    using System.Linq;
    using Sitecore.Foundation.SitecoreExtensions.Extensions;
    using System.Collections.Generic;
    using Sitecore.Feature.Identity.Models;
    using Sitecore.Data;
    using Sitecore.Data.Fields;

    public class FooterController : Controller
    {
        private readonly IFooterService _footerService;
        private readonly IHeaderService _headerService;

        public FooterController(IFooterService footerService, IHeaderService headerService)
        {
            this._footerService = footerService;
            this._headerService = headerService;
        }

        public ActionResult Copyright()
        {
            var datasource = this._footerService.GetCopyright();
            return this.View(datasource);
        }

        public ActionResult Footer()
        {
            var datasource = this._footerService.GetFooter();
            datasource.SocialIcons = datasource.IncludeHeaderSocialIcons ? _headerService.GetSocialIcons() : new SocialItems();
            return this.View(datasource);
        }

        public ActionResult MobileFooter()
        {
            var result = new List<MobileFooterModel>();
            var footer = this._footerService.GetFooter();
            foreach (Item group in footer.Menu.Children)
            {
                foreach (Item menus in group.Children.Where(x => x.IsDerived(Templates.FooterLink.ID)))
                {
                    foreach (Item item in menus.Children.Where(x => x.IsDerived(Templates.FooterLink.ID)))
                    {
                        if (item.GetBoolFieldValue(Templates.FooterLink.Fields.ShowOnMobile))
                        {
                            result.Add(new MobileFooterModel()
                            {
                                Name = item.DisplayName,
                                ItemUrl = item.LinkFieldUrl(Templates.FooterLink.Fields.Link),
                                IsExternalLink = IsExternalLink(item, Templates.FooterLink.Fields.Link)
                            });
                        }
                    }
                }
            }

            return this.View(result);
        }

        private bool IsExternalLink(Item item, ID fieldId)
        {
            var field = item.Fields[fieldId];
            LinkField linkField = (LinkField)field;
            return "external".Equals(linkField.LinkType, System.StringComparison.OrdinalIgnoreCase);
        }
    }
}