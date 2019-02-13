using Sitecore.Data;
using Sitecore.Feature.WebApi.Formatters;
using Sitecore.Feature.WebApi.Models;
using System;
using System.Web.Http;
using Sitecore.Foundation.ItemResolver.Extensions;
using Sitecore.Feature.WebApi.Services;
using Sitecore.Diagnostics;

namespace Sitecore.Feature.WebApi.Controllers
{
    [RoutePrefix(Constants.ApiRouting.Root)]
    public class StoreController : BaseApiController
    {
        private IStoreService StoreService { get; }

        public StoreController(IStoreService storeService) {
            this.StoreService = storeService;
        }
        [Route(Constants.ApiRouting.StoreList)]
        public IHttpActionResult GetStores()
        {
            try
            {
                this.GetPagingInfo(ref this.pageNo, ref this.pageSize);
                var output = new StoreInfoListOutput(Constants.ApiStatus.Success, this.StoreService.GetStores(string.Empty, pageNo, pageSize));
                return this.JsonResult<StoreInfoListOutput>(output);
            }
            catch (Exception ex)
            {
                Log.Error($":GetStores(). Error message: {ex.Message}", ex, this);
                var error = new JsonOutput(Constants.ApiStatus.Fail, ex.Message);
                return this.JsonResult<JsonOutput>(error);
            }
        }

        [Route(Constants.ApiRouting.StoreListForMall)]
        public IHttpActionResult GetStores(string mall)
        {
            try {

                this.GetPagingInfo(ref this.pageNo, ref this.pageSize);
                var output = new StoreInfoListOutput(Constants.ApiStatus.Success, this.StoreService.GetStores(mall, pageNo, pageSize));
                return this.JsonResult<StoreInfoListOutput>(output);
            }
            catch(Exception ex)
            {
                Log.Error($":GetStores({mall}). Error message: {ex.Message}", ex, this);
                var error = new JsonOutput(Constants.ApiStatus.Fail, ex.Message);
                return this.JsonResult<JsonOutput>(error);
            }
        }

        [Route(Constants.ApiRouting.StoreDetail)]
        public IHttpActionResult GetStore(string id)
        {
            try
            { 
                var item = Context.Database.GetItem(ID.Parse(id));
                if (!item.IsOnCurrentSite())
                {
                    throw new ArgumentException($"Store {id} is not found");
                }

                var output = new StoreOutput(Constants.ApiStatus.Success, new Store(item));
                return this.JsonResult<StoreOutput>(output);
            }
            catch (Exception ex)
            {
                Log.Error($":GetStore({id}). Error message: {ex.Message}", ex, this);
                var error = new JsonOutput(Constants.ApiStatus.Fail, ex.Message);
                return this.JsonResult<JsonOutput>(error);
            }
        }
    }
}