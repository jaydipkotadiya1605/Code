namespace Sitecore.Feature.Multisite.Services
{
    using Sitecore.ContentSearch.Utilities;
    using Sitecore.Data.Items;
    using Sitecore.Feature.Multisite.Models;
    using Sitecore.Foundation.Abstractions.SitecoreContext;
    using Sitecore.Foundation.DependencyInjection;
    using Sitecore.Foundation.Dictionary.Repositories;
    using Sitecore.Foundation.Search.Repositories;
    using Sitecore.Foundation.SitecoreExtensions.Extensions;
    using System.Collections.Generic;
    using System.Linq;

    [Service(typeof(IMultisiteService))]
    public class MultisiteService : IMultisiteService
    {
        public ISitecoreContext SitecoreContext { get; }
        public IMallSearchRepository MallSearchRes { get; }
        public MultisiteService(ISitecoreContext SitecoreContext, IMallSearchRepository mallSearchRes)
        {
            this.MallSearchRes = mallSearchRes;
            this.SitecoreContext = SitecoreContext;
        }

        public IEnumerable<MallViewModel> GetAllMalls()
        {
            var model = this.MallSearchRes.GetAllMalls()?.ScoredItems
                .Select(x => new MallViewModel
                {
                    Id = IdHelper.NormalizeGuid(x.Item.ID),
                    Name = x.Item.DisplayName
                })
                .OrderBy(x => x.Name)
                .ToList();
            if (model != null)
            {
                model.Insert(0, new MallViewModel
                {
                    Id = string.Empty,
                    Name = DictionaryPhraseRepository.Current.Get(Multisite.Constants.AllProperties, Multisite.Constants.AllPropertiesText)
                });
            }
            return model;
        }

        public SitesViewModel GetSites() {
            var sitesViewModel = new SitesViewModel()
            {
                CommercialMenuSites = new List<CommercialMenuSite>(),
                MallMenuSites = new List<MallMenuSite>()
            };
            var fraserRewardItem = SitecoreContext.Database.GetItem(Templates.FrasersRewards.ID);
            if (fraserRewardItem != null)
            {
                this.GetMallSiteAndCommerceSite(fraserRewardItem, sitesViewModel);
                sitesViewModel.PageSizeCommerceSite = sitesViewModel.CommercialMenuSites.Count <= 1 ? sitesViewModel.CommercialMenuSites.Count : sitesViewModel.CommercialMenuSites.Count/2;
                sitesViewModel.PageSizeMallMenuSite = sitesViewModel.MallMenuSites.Count <= 1 ? sitesViewModel.MallMenuSites.Count : sitesViewModel.MallMenuSites.Count / 2;
                sitesViewModel.RepeatLoopCommerceSite = CalculateRepeatLoop(sitesViewModel.PageSizeCommerceSite, sitesViewModel.CommercialMenuSites.Count);
                sitesViewModel.RepeatLoopMallSites = CalculateRepeatLoop(sitesViewModel.PageSizeMallMenuSite, sitesViewModel.MallMenuSites.Count);
                var siteItem = SitecoreContext.Database.GetItem(Sitecore.Context.Site.StartPath);
                if (siteItem.Parent.IsDerived(Templates.CommercialWebsite.ID) || siteItem.Parent.IsDerived(Templates.MallWebsite.ID))
                {
                    sitesViewModel.GroupMenuName = siteItem.Parent.GetString(Multisite.Templates.Identity.Fields.SiteName);
                }
                else
                {
                    sitesViewModel.GroupMenuName = DictionaryPhraseRepository.Current.Get(Multisite.Constants.SiteSwitcher, Multisite.Constants.SiteSwitcherText);
                }
            }
            return sitesViewModel;
        }

        private void GetMallSiteAndCommerceSite(Item fraserRewardItem, SitesViewModel sitesViewModel)
        {
            var siteInfoList = Sitecore.Configuration.Factory.GetSiteInfoList();
            foreach (Item item in fraserRewardItem.Parent.Children)
            {
                var siteInfo = siteInfoList.FirstOrDefault(x => item.Paths.FullPath.Trim().Equals(x.RootPath.Trim(), System.StringComparison.OrdinalIgnoreCase));
                if (siteInfo != null)
                {
                    var schema = System.Web.HttpContext.Current.Request.Url.Scheme;

                    if (item.IsDerived(Templates.MallWebsite.ID))
                    {
                        sitesViewModel.MallMenuSites.Add(new MallMenuSite()
                        {
                            SiteTitle = item.Field(Multisite.Templates.Identity.Fields.SiteName),
                            Link = $"{schema}://{siteInfo.HostName}"
                        });
                    }
                    else if (item.IsDerived(Templates.CommercialWebsite.ID))
                    {
                        sitesViewModel.CommercialMenuSites.Add(new CommercialMenuSite()
                        {
                            SiteTitle = item.Field(Multisite.Templates.Identity.Fields.SiteName),
                            Link = $"{schema}://{siteInfo.HostName}"
                        });
                    }
                }
            }
        }

        private int CalculateRepeatLoop(int pageSize, int totalItem)
        {
            if (totalItem == 0)
                return 0;
            int repeat = totalItem / pageSize;
            if (totalItem % pageSize != 0)
                repeat++;
            return repeat;
        }
    }
}