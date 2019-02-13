using Sitecore.Feature.WebApi.Models;
using Sitecore.Foundation.DependencyInjection;
using Sitecore.Foundation.Search.Repositories;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Foundation.Search.Models.Index;
using Sitecore.ContentSearch.Linq.Utilities;
using FraserContent = Sitecore.Foundation.FrasersContent;
using Sitecore.ContentSearch.Utilities;

namespace Sitecore.Feature.WebApi.Services
{
    [Service(typeof(IStoreService))]
    public class StoreService : IStoreService
    {
        private IStoreSearchRepository StoreSearchRepository { get; }
        private ISiteSearchRepository SiteSearchRepository { get; }
        public StoreService(IStoreSearchRepository storeSearchRepository, ISiteSearchRepository siteSearchRepository)
        {
            this.StoreSearchRepository = storeSearchRepository;
            this.SiteSearchRepository = siteSearchRepository;
        }

        public List<StoreInfo> GetStores(string siteCode, int pageNo, int pageSize)
        {
            var query = PredicateBuilder.True<StoreIndex>();
            query = query.And(x => x.AllTemplates.Contains(IdHelper.NormalizeGuid(FraserContent.Templates.StorePage.ID)));
            if (!string.IsNullOrEmpty(siteCode))
            {
                var siteItems = SiteSearchRepository.GetSitebySiteCode(siteCode);
                if (siteItems.TotalNumberOfResults == 1)
                {
                    var mallId = siteItems.ScoredItems.First().Item.ID;
                    query = query.And(x => x.MallSite.Contains(IdHelper.NormalizeGuid(mallId)));
                }
                else
                    return new List<StoreInfo>();
            }

            var results = this.StoreSearchRepository.Search(query, pageNo, pageSize,
                    queryable => queryable.OrderByDescending(x => x[FraserContent.Templates.Store.Fields.Score_FieldName]).ThenBy(x => x.StoreName));
            return results.ScoredItems.Select(r => new StoreInfo(r.Item))?.ToList();
        }
    }
}