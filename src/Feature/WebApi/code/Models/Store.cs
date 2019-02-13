using Newtonsoft.Json;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using System.Collections.Generic;
using System.Linq;
using FraserContent = Sitecore.Foundation.FrasersContent;

namespace Sitecore.Feature.WebApi.Models
{
    public class Store
    {
        [JsonProperty(Order = 1, PropertyName = "id")]
        public ID Id { get; set; }
        [JsonProperty(Order=2, PropertyName = "store_name")]
        public string Name { get; set; }
        [JsonProperty(Order=3, PropertyName = "unit_no")]
        public string UnitNo { get; set; }
        [JsonProperty(Order=4, PropertyName = "phone_no")]
        public string PhoneNo { get; set; }
        [JsonProperty(Order=5, PropertyName = "logo")]
        public string Logo { get; set; }
        [JsonProperty(Order=6, PropertyName = "opening_hours")]
        public string OpeningHours { get; set; }
        [JsonProperty(Order=7, PropertyName = "description")]
        public string Description { get; set; }
        [JsonProperty(Order=8, PropertyName = "halal")]
        public bool? Halal { get; set; }
        [JsonProperty(Order = 9, PropertyName = "gift_card")]
        public bool? GilfCard { get; set; }
        [JsonProperty(Order = 10, PropertyName = "frasers_point")]
        public bool? FrasersPoint { get; set; }
        [JsonProperty(Order = 11, PropertyName = "share_link")]
        public string ShareLink { get; set; }

        public Store()
        {
            this.Id = null;
            this.Name = null;
            this.UnitNo = null;
            this.PhoneNo = null;
            this.Logo = null;
            this.OpeningHours = null;
            this.Description = null;
            this.Halal = null;
            this.GilfCard = null;
            this.FrasersPoint = null;
            this.ShareLink = null;
        }

        public Store(Item item)
        {
            if (item == null)
            {
                return;
            }

            this.Id = item.ID;
            this.Name = item.Fields[Templates.Store.StoreName].Value;
            this.UnitNo = item.Fields[Templates.Store.UnitNo].Value;
            this.PhoneNo = item.Fields[Templates.Store.PhoneNo].Value;

            // Get Logo image
            this.Logo = item.ImageUrl(Templates.Store.Logo);

            this.OpeningHours = item.Fields[Templates.Store.OpeningHours].Value;
            this.Description = item.Fields[Templates.Store.Description].Value;

            // Get Halal / GilfCard / FrasersPoint
            MultilistField storeOffers = item.Fields[Templates.Store.StoreOffers];
            if(storeOffers != null)
            {
                var items = storeOffers.GetItems();
                this.Halal = items.Count(x => x.ID.ToString().Equals(Templates.StoreOffers.HalalCertified.ToString())) == 1;
                this.GilfCard = items.Count(x => x.ID.ToString().Equals(Templates.StoreOffers.AcceptsGiftCards.ToString())) == 1;
                this.FrasersPoint = items.Count(x => x.ID.ToString().Equals(Templates.StoreOffers.EarnFrasersPoints.ToString())) == 1;
            }

            // Get ShareLink
            this.ShareLink = item.Url(Constants.DefaultUrlOptions);
        }
    }

    public class StoreInfo
    {
        [JsonProperty(Order = 1, PropertyName = "id")]
        public ID Id { get; set; }
        [JsonProperty(Order = 2, PropertyName = "store_name")]
        public string Name { get; set; }
        [JsonProperty(Order = 3, PropertyName = "logo")]
        public string Logo { get; set; }
        [JsonProperty(Order = 4, PropertyName = "summary")]
        public string Summary { get; set; }
        [JsonProperty(Order = 5, PropertyName = "unit_no")]
        public string UnitNo { get; set; }
        [JsonProperty(Order = 6, PropertyName = "categories")]
        public string[] Categories { get; set; }
        [JsonProperty(Order = 7, PropertyName = "share_link")]
        public string ShareLink { get; set; }

        public StoreInfo()
        {
            this.Categories = (new List<string>()).ToArray();
        }

        public StoreInfo(Item item)
        {
            if (item == null)
            {
                return;
            }

            this.Id = item.ID;
            this.Name = item.Fields[Templates.Store.StoreName].Value;
            this.UnitNo = item.Fields[Templates.Store.UnitNo].Value;

            // Get Logo image
            this.Logo = item.ImageUrl(Templates.Store.Logo);
            this.Summary = item.Fields[Templates.Store.Description].Value;

            // Get ShareLink
            this.ShareLink = item.Url(Constants.DefaultUrlOptions);

            // Get Categories
            var categories = item.GetMultiListValueItems(FraserContent.Templates.Store.Fields.StoreCategories);
            this.Categories = categories.Select(x => x.GetString(FraserContent.Templates.StoreCategory.Fields.Value)).ToArray();
        }
    }
}