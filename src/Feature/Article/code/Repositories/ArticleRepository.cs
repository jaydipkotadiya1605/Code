namespace Sitecore.Feature.Article.Repositories
{
    using Sitecore.Data.Items;
    using System.Linq;
    using Sitecore.Feature.Article.Models;
    using Sitecore.Foundation.SitecoreExtensions.Extensions;
    using Sitecore.Foundation.DependencyInjection;
    using FrasersContent = Foundation.FrasersContent;
    using Sitecore.Foundation.Search.Repositories;
    using Sitecore.ContentSearch.Utilities;
    using Sitecore.Foundation.Abstractions.SitecoreContext;
    using Multisite = Foundation.Multisite;
    using System.Collections.Generic;
    using System;
    using Sitecore.Data;

    [Service(typeof(IArticleRepository))]
    public class ArticleRepository : IArticleRepository
    {
        public IArticleSearchRepository ArticleSearchRepository { get; }
        private readonly ISitecoreContext _sitecoreContext;

        public ArticleRepository(ISitecoreContext sitecoreContext, IArticleSearchRepository articleSearchRepository)
        {
            this._sitecoreContext = sitecoreContext;
            this.ArticleSearchRepository = articleSearchRepository;
        }

        public ArticleItems GetArticles(string category, int page = 0, int pageSize = 100)
        {
            var path = _sitecoreContext.Site.RootPath;
            var categoryNames = new List<string>();
            if (!string.IsNullOrEmpty(category))
                categoryNames.Add(category);
            var result = ArticleSearchRepository.GetArticleItems(categoryNames, string.Empty, page, pageSize, path);
            var hasMoreResult = pageSize < result.TotalNumberOfResults;

            return new ArticleItems()
            {
                Items = result.ScoredItems.Select(x => CreateArticleItem(x.Item)).Where(x => x != null).ToList(),
                HasMoreResult = hasMoreResult,
                PageSize = pageSize
            };
        }

        public ArticleWidgetItems GetMostWantedDeals(string listPageItemId, string strIsInStorePage, string categories, int page = 0, int pageSize = 100)
        {
            var path = _sitecoreContext.Site.RootPath;
            var categoryNames = GetListCategoryNames(categories);

            string storeId = string.Empty;
            bool isInStorePage = strIsInStorePage == FrasersContent.Constants.ValueIsTrue;
            if (isInStorePage)
            {
                var storeItem = _sitecoreContext.Item;
                if (storeItem != null && storeItem.IsDerived(FrasersContent.Templates.Store.ID))
                {
                    storeId = IdHelper.NormalizeGuid(storeItem.ID);
                }
            }

            var result = ArticleSearchRepository.GetArticleItems(categoryNames, storeId, page, pageSize, path);
            var hasMoreResult = pageSize < result.TotalNumberOfResults;

            return new ArticleWidgetItems()
            {
                Items = result.ScoredItems.Select(x => CreateArticleWidgetItem(x.Item)).Where(x => x != null).ToList(),
                LinkOfListPage = GetLinkListPage(listPageItemId),
                HasMoreResult = hasMoreResult
            };
        }

        private ArticleWidgetItem CreateArticleWidgetItem(Item item)
        {
            if (item != null && item.IsDerived(FrasersContent.Templates.Article.ID))
            {
                var thumbnailField = ((Data.Fields.ImageField)item.Fields[FrasersContent.Templates.Article.Fields.Thumbnail]);
                string altText = thumbnailField.Alt;
                var widgetItem = new ArticleWidgetItem
                {
                    Item = item,
                    Title = item.Fields[FrasersContent.Templates.Article.Fields.Title].Value,
                    Url = item.Url(),
                    Alt = string.IsNullOrEmpty(altText) ? item.Fields[FrasersContent.Templates.Article.Fields.Title].Value : altText

                };
                var storeItem = item.GetDroplinkItem(FrasersContent.Templates.Article.Fields.Store);
                if (storeItem != null)
                {
                    widgetItem.StoreName = storeItem.GetString(FrasersContent.Templates.Store.Fields.StoreName);
                    widgetItem.StoreUrl = storeItem.Url();
                }

                return widgetItem;
            }
            return null;
        }

        private ArticleItem CreateArticleItem(Item item)
        {
            if (item != null && item.IsDerived(FrasersContent.Templates.Article.ID))
            {
                var thumbnailField = ((Data.Fields.ImageField)item.Fields[FrasersContent.Templates.Article.Fields.Thumbnail]);
                string altText = thumbnailField.Alt;

                return new ArticleItem
                {
                    Item = item,
                    Id = IdHelper.NormalizeGuid(item.ID),
                    Title = item.Fields[FrasersContent.Templates.Article.Fields.Title].Value,
                    EndDate = DateUtil.ToServerTime(item.GetDateTime(FrasersContent.Templates.Article.Fields.EndDate)),
                    StartDate = DateUtil.ToServerTime(item.GetDateTime(FrasersContent.Templates.Article.Fields.StartDate)),
                    Url = item.Url(),
                    Alt = string.IsNullOrEmpty(altText) ? item.Fields[FrasersContent.Templates.Article.Fields.Title].Value : altText
                };
            }
            return null;
        }

        private string GetLinkListPage(string itemID)
        {
            if (itemID != null)
            {
                var item = _sitecoreContext.Database.GetItem(new Data.ID(itemID));
                if (item != null)
                {
                    return item.IsDerived(FrasersContent.Templates.Link.ID) ? item.LinkFieldUrl(FrasersContent.Templates.Link.Fields.Link) : item.Url();
                }
            }
            return FrasersContent.Constants.DefaultLink;
        }

        private List<string> GetListCategoryNames(string categories)
        {
            var categoryNames = new List<string>();
            var separator = new[] { "|" };
            var categoryIds = categories != null
                    ? categories.Split(separator, StringSplitOptions.RemoveEmptyEntries).ToList()
                    : new List<string>();
            foreach (var id in categoryIds)
            {
                var name = _sitecoreContext.Database.GetItem(new ID(id))?.DisplayName ?? string.Empty;
                categoryNames.Add(name);
            }

            return categoryNames;
        }

    }
}