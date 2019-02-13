namespace Sitecore.Foundation.SitecoreExtensions.Repositories
{
    using Sitecore.Data.Fields;
    using Sitecore.Data.Items;
    using Sitecore.Diagnostics;
    using Sitecore.Foundation.DependencyInjection;
    using Sitecore.Mvc.Presentation;
    using Sitecore.Reflection;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    [Service(typeof(IRenderingPropertiesRepository))]
    public class RenderingPropertiesRepository : IRenderingPropertiesRepository
    {
        public T Get<T>(Rendering rendering)
        {
            var obj = ReflectionUtil.CreateObject(typeof(T));
            var currentContext = rendering;
            var parameters = currentContext?.Properties["Parameters"];
            if (parameters == null)
                return (T)obj;

            parameters = this.FilterEmptyParametrs(parameters);
            try
            {
                ReflectionUtil.SetProperties(obj, parameters);
            }
            catch (Exception e)
            {
                Log.Error(e.Message, e, this);
            }

            return (T)obj;
        }

        public T GetExt<T>(Rendering rendering)
        {
            var result = this.Get<T>(rendering);
            try
            {
                var parameters = this.FilterEmptyParametrs(HttpContext.Current.Request.QueryString.ToString());
                ReflectionUtil.SetProperties(result, parameters);
            }
            catch (Exception e)
            {
                Log.Error(e.Message, e, this);
            }

            return result;
        }

        protected virtual string FilterEmptyParametrs(string parameters)
        {
            parameters = HttpUtility.UrlDecode(parameters);
            var parametersList = parameters.Split(new[] { '&', '#' }, StringSplitOptions.RemoveEmptyEntries);

            List<string> paramList = new List<string>();
            foreach (var eachParams in parametersList)
            {
                if (eachParams.ToLower().Contains("storeofferid") && eachParams.Contains("="))
                {
                    paramList.Add(AddLinkedStoreOffers(eachParams));
                }
                else
                {
                    paramList.Add(eachParams);
                }
            }

            return string.Join("&", paramList.Where(x => x.Contains("=")));
        }

        private string AddLinkedStoreOffers(string paramList)
        {
            string returnValue = string.Empty;
            string offerIdQuerStringParameter = paramList.Split('=')[0];
            string offerIds = paramList.Split('=')[1];
            List<string> offerIdsList = new List<string>();
            List<string> offerIdsListUpdated = new List<string>();

            if (!string.IsNullOrWhiteSpace(offerIds))
            {
                offerIdsList = offerIds.Split(',')?.ToList();
            }

            foreach (var eachParam in offerIdsList)
            {
                var offerItem = Sitecore.Context.Database.GetItem(new Data.ID(eachParam));
                if (offerItem != null)
                {
                    List<Item> linkedOffers = ((MultilistField)offerItem.Fields[Constants.LinkedOffers])?.GetItems()?.ToList();
                    if (linkedOffers != null && linkedOffers.Count > 0)
                    {
                        //linkedOffers.Add(offerItem);
                        string linkedOfferIds = string.Join(",", linkedOffers.Select(x => ContentSearch.Utilities.IdHelper.NormalizeGuid(x.ID)));
                        offerIdsListUpdated.Add(linkedOfferIds);
                    }
                    else
                    {
                        offerIdsListUpdated.Add(eachParam);
                    }
                }
                else
                {
                    offerIdsListUpdated.Add(eachParam);
                }
            }

            returnValue = offerIdQuerStringParameter + "=" + string.Join(",", offerIdsListUpdated);

            return returnValue;
        }
    }
}