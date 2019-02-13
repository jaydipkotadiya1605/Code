namespace Sitecore.Feature.Store
{
    using System;
    using System.Collections.Generic;
    using Sitecore.ContentSearch.Utilities;
    using Sitecore.Data;

    public static class Constants
    {
        /* Dictionary path */
        public static readonly string All = "/Stores/All";
        public static readonly string AllText = "All";
        public static readonly string Category = "/Stores/Category";
        public static readonly string CategoryText = "Category";
        public static readonly string Wing = "/Stores/Wing";
        public static readonly string WingText = "Wing";
        public static readonly string NewStore = "/Stores/New Store";
        public static readonly string NewStoreText = "New Store";
        public static readonly string FindUs = "/Stores/Find Us";
        public static readonly string FindUsText = "FIND US";
        public static readonly string CallUs = "/Stores/Call Us";
        public static readonly string CallUsText = "CALL US";
        public static readonly string OpeFrom = "/Stores/Open From";
        public static readonly string OpenFromText = "OPEN FROM";
        public static readonly string SearchNotFound = "/Stores/Not Found";
        public static readonly string SearchNotFoundText = "Sorry, there are no stores matching the filters set.";
        public static readonly string LoadMore = "/Stores/Load More";
        public static readonly string LoadMoreText = "Load More";
        public static readonly string StoreQuickFinder = "/Stores/Store Quick Finder";
        public static readonly string StoreQuickFinderText = "Store Quick Finder";
        public static readonly string Search = "/Stores/Search";
        public static readonly string SearchText = "Search";
        public static readonly string SearchStore = "/Stores/Search Store";
        public static readonly string SearchStoreText = "Search Store";
        public static readonly string RelatedStores = "/Stores/Related Stores";
        public static readonly string RelatedStoresText = "Stores You Might Like";
        public static readonly string Stores = "/Stores/Stores";
        public static readonly string StoresText = "Stores";
        public static readonly string StoreName = "/Stores/Store Name";
        public static readonly string StoreNameText = "Store Name";
        public static readonly string FilterBy = "/Stores/Filter By";
        public static readonly string FilterByText = "FILTER BY";
        public static readonly string MoreFilters = "/Stores/More Filters";
        public static readonly string MoreFiltersText = "MORE FILTERS";

        public static readonly string StoreOfferNewStatus = IdHelper.NormalizeGuid(Guid.NewGuid().ToString());
        public static readonly ID AlphabetDatasources = new ID("{BB0060F3-3376-496F-A9E5-472B32CFCFFA}");
        public static readonly string ContentType = "/Stores/Content Type";
        public static readonly string ContentTypeName = "Store";
        public static readonly string MallRenderingId = "{EF4DD19F-F5B3-4D10-9BDF-B1976FAD7F00}";
        public static readonly string StoreAlphabetRenderingId = "{773B230D-BFD5-414A-B5AD-B589AF41564F}";
        public static readonly string StoreCategoryRenderingId = "{FFC6D6CE-CBBD-40A0-8610-31F5FE3019CD}";
        public static readonly string StoreOfferRenderingId = "{514C5950-7FB9-46FA-B1E0-160965DB63F4}";
        public static readonly string StoreWingRenderingId = "{8B857E61-7855-43C0-8162-E83A5DB87988}";
        public static readonly string StoreCategoryDatasource = "{5415EFC9-AD12-410C-AFCD-59A644E1A7B7}";
        public static readonly string StoreWingDatasource = "{81A4C924-32EA-4212-A6BD-255A3114532F}";
        public static readonly string StoreOfferDatasource = "{D6CFDCDC-5437-4C86-A57D-18A402ACF666}";

        public static IList<string> FieldNames => fieldNames;

        private static readonly IList<string> fieldNames = new[]
        {
            Foundation.FrasersContent.Templates.Store.Fields.StoreName_FieldName,
            Foundation.FrasersContent.Templates.Store.Fields.PhoneNumber_FieldName,
            Foundation.FrasersContent.Templates.Store.Fields.NewDate_FieldName,
            Foundation.FrasersContent.Templates.Store.Fields.StoreCategories_FieldName,
            Foundation.FrasersContent.Templates.Store.Fields.Description_FieldName,
            Foundation.FrasersContent.Templates.Store.Fields.Wing_FieldName,
            Foundation.FrasersContent.Templates.Store.Fields.UnitNo_FieldName,
            Foundation.FrasersContent.Templates.Store.Fields.Contact_FieldName,
            Foundation.FrasersContent.Templates.Store.Fields.OpeningHours_FieldName,
            Foundation.FrasersContent.Templates.Store.Fields.StoreOffers_FieldName,
            Foundation.FrasersContent.Templates.Store.Fields.Brands_FieldName,
            Foundation.FrasersContent.Templates.Store.Fields.Keywords_FieldName
        };

        public struct QueryString
        {
            public static string Category => "category";
            public static string PageIndex => "pageIndex";
            public static string MallId => "mallId";
        }
    }
}
