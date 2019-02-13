using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Foundation.FrasersContent;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Caching;

namespace Sitecore.Foundation.Search.Models.Index.Fields
{
    /// <summary>
    /// ComputedField for PageViews
    /// </summary>
    public class PageViewsComputedField : IComputedIndexField
    {
        private static ID[] allowTemplates = new ID[] { Templates.Article.ID,
                                                        Templates.Event.ID,
                                                        Templates.SpecialEvent.ID,
                                                        Templates.Blog.ID,
                                                        Templates.Store.ID };
        private readonly string cacheKey = $"cache{nameof(PageViewsComputedField)}";
        private readonly static object cacheLock = new object();

        /// <summary>
        /// Compute field value
        /// </summary>
        /// <param name="indexable">indexable</param>
        /// <returns></returns>
        public object ComputeFieldValue(IIndexable indexable)
        {
            Item item = indexable as SitecoreIndexableItem;
            if (item == null)
            {
                return null;
            }
            
            if (!allowTemplates.Contains(item.TemplateID) 
               || item.Fields[Sitecore.FieldIDs.LayoutField] == null
               || string.IsNullOrEmpty(item.Fields[Sitecore.FieldIDs.LayoutField].Value))
            {
                return null;
            }

            return GetPageViewsForItem(item.ID);
        }

        /// <summary>
        /// Get PageViews for item
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        private int GetPageViewsForItem(ID itemId)
        {
            Dictionary<ID, int> pageViews = GetCachedPageViews();
            if (pageViews == null)
            {
                return 0;
            }
            else if (!pageViews.ContainsKey(itemId))
            {
                return 0;
            }
            return pageViews[itemId];
        }

        /// <summary>
        /// Get all PageViews
        /// </summary>
        /// <returns></returns>
        private Dictionary<ID, int> GetCachedPageViews()
        {
            var pageViews = MemoryCache.Default.Get(cacheKey, null) as Dictionary<ID, int>;

            if (pageViews != null)
            {
                return pageViews;
            }

            lock (cacheLock)
            {
                pageViews = MemoryCache.Default.Get(cacheKey, null) as Dictionary<ID, int>;
                if (pageViews != null)
                {
                    return pageViews;
                }

                //query analytics database
                pageViews = GetAllItemsFromAnalytics();

                CacheItemPolicy cacheItemPolicy = new CacheItemPolicy()
                {
                    AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddMinutes(5))
                };

                if (pageViews == null)
                {
                    pageViews = new Dictionary<ID, int>();
                }
                MemoryCache.Default.Set(cacheKey, pageViews, cacheItemPolicy);

                return pageViews;
            }
        }

        /// <summary>
        /// Query PageViews from Analytics database
        /// </summary>
        /// <returns></returns>
        private Dictionary<ID, int> GetAllItemsFromAnalytics()
        {
            Dictionary<ID, int> dictionary = new Dictionary<ID, int>();

            string getAllPageViewsQuery = @"SELECT [ItemId], Sum([Views]) FROM [Fact_PageViews] GROUP BY [ItemId]";
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["reporting"].ConnectionString))
            {
                var command = new SqlCommand(getAllPageViewsQuery, connection);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dictionary.Add(ID.Parse(reader.GetGuid((int)PageViewsColumn.ItemId)),
                        int.Parse(reader[(int)PageViewsColumn.Views].ToString()));
                    }
                }
                connection.Close();
            }
            return dictionary;
        }

        public string FieldName { get; set; }
        public string ReturnType { get; set; }

        private enum PageViewsColumn
        {
            ItemId = 0,
            Views = 1
        }
    }
}