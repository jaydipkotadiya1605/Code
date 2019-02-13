using Newtonsoft.Json;
using Sitecore.Feature.WebApi.Formatters;
using System.Text;
using System.Web.Http;
using System.Web.Http.Results;

namespace Sitecore.Feature.WebApi.Controllers
{
    public class BaseApiController : ApiController
    {
        protected int pageNo;
        protected int pageSize;
        public BaseApiController() {
            this.pageNo = 0;
            this.pageSize = 0;
        }
        private static JsonSerializerSettings JsonSerializerSettings => new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DateFormatHandling = DateFormatHandling.IsoDateFormat
        };

        private static Encoding Encoding => Encoding.UTF8;

        protected JsonResult<T> JsonResult<T>(T output) where T : JsonOutput
        { 
            return new JsonResult<T>(output, JsonSerializerSettings, Encoding, this);
        }

        protected void GetPagingInfo(ref int pageNo, ref int pageSize)
        {
            pageNo = int.TryParse(Context.Request.GetQueryString(Constants.ApiParametter.PageNo), out pageNo) ? int.Parse(Context.Request.GetQueryString(Constants.ApiParametter.PageNo)) : Constants.DefaultPageNoApi;
            pageSize = int.TryParse(Context.Request.GetQueryString(Constants.ApiParametter.PageSize), out pageSize) ? int.Parse(Context.Request.GetQueryString(Constants.ApiParametter.PageSize)) : Constants.DefaultPageSizeApi;
        }
    }
}