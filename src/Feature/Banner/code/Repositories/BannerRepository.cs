using Sitecore.Data.Items;
using Sitecore.Feature.Banner.Models;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Feature.Banner.Extentions;
using System.Linq;
using Sitecore.Foundation.Search.Repositories;
using FrasersContent = Sitecore.Foundation.FrasersContent;
using Sitecore.Foundation.DependencyInjection;

namespace Sitecore.Feature.Banner.Repositories
{
    [Service(typeof(IBannerRepository))]
    public class BannerRepository : IBannerRepository
    {
        public IBannerSearchRepository BannerSearchRepository { get; }

        public BannerRepository(IBannerSearchRepository bannerSearchRepository)
        {
            this.BannerSearchRepository = bannerSearchRepository;
        }

        public BannerItems GetBanner()
        {
            var rootItem = Context.Site.GetRootItem();
            
            var bannerItems = BannerSearchRepository.GetBannerItems(rootItem.ID).ScoredItems
                            .Select(x => this.CreateBannerItem(x.Item));

            return new BannerItems
            {
                Items = bannerItems.ToList(),
                MiniItems = bannerItems.Map(x => x.Item = null).ToList()
            };
        }

        private BannerItem CreateBannerItem(Item item)
        {
            if (item != null && item.IsDerived(FrasersContent.Templates.Banner.ID))
            {
                return new BannerItem
                {
                    Item = item,
                    Title = item.Fields[FrasersContent.Templates.Banner.Fields.Title].Value,
                    Summary = item.Fields[FrasersContent.Templates.Banner.Fields.Summary].Value,
                    Link = item.LinkFieldUrl(FrasersContent.Templates.Banner.Fields.Link),
                    Category = item.GetDroplinkItem(FrasersContent.Templates.Banner.Fields.Category)?.DisplayName ?? string.Empty
                };
            }
            return null;
        }
    }
}