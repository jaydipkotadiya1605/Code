using Sitecore.Data;
using Sitecore.Feature.WebApi.Models;
using Sitecore.Foundation.DependencyInjection;
using Sitecore.Foundation.Search.Repositories;
using System.Collections.Generic;
using System.Linq;
using FraserContent = Sitecore.Foundation.FrasersContent;

namespace Sitecore.Feature.WebApi.Services
{
    [Service(typeof(IBannerService))]
    public class BannerService : IBannerService
    {
        private IBannerSearchRepository BannerSearchRepository { get; }

        public BannerService(IBannerSearchRepository bannerSearchRepository)
        {
            this.BannerSearchRepository = bannerSearchRepository;
        }

        public List<Banner> GetBanners(string mallId, int pageNo, int pageSize)
        {
            var itemId = string.IsNullOrEmpty(mallId) ? FraserContent.Templates.FrasersRewards.ID : new ID(mallId);

            var bannerItems = this.BannerSearchRepository.GetBannerItems(itemId, pageNo, pageSize).ScoredItems
                           .Select(x => new Banner(x.Item));

            return bannerItems.ToList();
        }

        public List<Banner> GetBannersBySitecode(string siteCode, int pageNo, int pageSize)
        {
            if (string.IsNullOrEmpty(siteCode))
            {
                return new List<Banner>();
            }
            var bannerItems = this.BannerSearchRepository.GetBannerBySiteCode(siteCode, pageNo, pageSize).ScoredItems
                           .Select(x => new Banner(x.Item));

            return bannerItems.ToList();
        }
    }
}